﻿using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forum.Domain.Monitoring;

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

            logger.LogInformation("Command successfully handled {Command}", request);
            monitoredRequest.MonitorSuccess(metrics);
            activity?.AddTag("error", false);

            return result;
        }
        catch (Exception ex)
        {
            monitoredRequest.MonitorFailure(metrics);
            activity?.AddTag("error", true);
            logger.LogError(ex, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}