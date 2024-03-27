using Domain.Monitoring;
using MediatR;

namespace Domain.UseCases.SignOut;

public record SignOutCommand : IRequest, IMonitoredRequest
{
    private const string CounterName = "user.sign_out";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}