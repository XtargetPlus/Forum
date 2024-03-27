using Domain.Authentication;
using Domain.Monitoring;
using MediatR;

namespace Domain.UseCases.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>, IMonitoredRequest
{
    private const string CounterName = "user.sign_on";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}