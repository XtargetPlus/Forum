using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API.Monitoring;

internal static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services, IConfigurationManager configuration) => services
        .AddOpenTelemetry()
        .WithMetrics(builder => builder
            .AddAspNetCoreInstrumentation()
            .AddMeter("Domain")
            .AddPrometheusExporter()
            .AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new[] {0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 10}
            }))
        .WithTracing(builder => builder
            .ConfigureResource(r => r.AddService("Forum"))
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(cfg => cfg.SetDbStatementForText = true)
            .AddSource("Domain")
            .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)))
        .Services;
}
