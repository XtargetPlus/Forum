using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace Search.ForumConsumer.Monitoring;

internal static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddApiLogging(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
        services.AddSingleton(loggingLevelSwitch);

        return services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .Enrich.WithProperty("Application", "Search.ForumConsumer")
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Enrich.With<TracingContextEnricher>()
                .WriteTo.OpenSearch(
                    configuration.GetConnectionString("OpenSearch"),
                    "search-logs-{0:yyyy.MM.dd}")
                .WriteTo.GrafanaLoki(
                    configuration.GetConnectionString("Loki")!,
                    propertiesAsLabels: new[]
                    {
                        "Environment", "Application"
                    },
                    leavePropertiesIntact: true))
            .CreateLogger()));
    }
}

internal class TracingContextEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current ?? default;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity?.TraceId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity?.SpanId));
    }
}