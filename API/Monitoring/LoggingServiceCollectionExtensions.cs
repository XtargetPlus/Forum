using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace API.Monitoring;

internal static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddApiLogging(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment) =>
        services.AddLogging(b => b
            .Configure(opt => opt.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId)
            .AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "Forum.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .Enrich.With<TracingContextEnricher>()
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.OpenSearch(
                        configuration.GetConnectionString("OpenSearch"),
                        "forum-logs-{0:yyyy.MM.dd}")
                    .WriteTo.GrafanaLoki(
                        configuration.GetConnectionString("Loki")!,
                        propertiesAsLabels: new[]
                        {
                            "level", "Environment", "Application", "SourceContext"
                        },
                        leavePropertiesIntact: true))
                .CreateLogger()));

    private class TracingContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;
            if (activity is null) return;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity.TraceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId));
        }
    }
}