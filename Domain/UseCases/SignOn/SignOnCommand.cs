using Forum.Domain.Authentication;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>, IMonitoredRequest
{
    private const string CounterName = "user.sign_on";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}