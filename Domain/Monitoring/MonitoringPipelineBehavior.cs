using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Monitoring;

internal class MonitoringPipelineBehavior<TRequest, TResponse>(
    DomainMetrics metrics,
    ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();

        using var activity = DomainMetrics.ActivitySource.StartActivity("usecase", ActivityKind.Internal, default(ActivityContext));

        activity?.AddTag("forum.command", request.GetType().Name);

        try
        {
            var result = await next.Invoke();
            monitoredRequest.MonitorSuccess(metrics);
            return result;
        }
        catch (Exception ex)
        {
            monitoredRequest.MonitorFailure(metrics);
            logger.LogError(ex, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}