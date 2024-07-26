using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Search.ForumConsumer.Monitoring;

internal static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services, IConfigurationManager configuration) => services
        .AddOpenTelemetry()
        .WithMetrics(builder => builder
            .AddAspNetCoreInstrumentation()
            .AddPrometheusExporter()
            .AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new[] {0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 10}
            }))
        .WithTracing(builder => builder
            .ConfigureResource(r => r.AddService("Search.ForumConsumer"))
            .AddAspNetCoreInstrumentation(opt =>
            {
                opt.Filter += context => 
                    !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                    !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                opt.EnrichWithHttpResponse = (activity, response) =>
                    activity.AddTag("error", response.StatusCode >= 400);
            })
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource(Metrics.ApplicationName)
            .AddJaegerExporter(cfg => cfg.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)))
        .Services;
}
