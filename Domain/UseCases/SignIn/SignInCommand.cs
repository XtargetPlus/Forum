using Forum.Domain.Authentication;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(IIdentity identity, string token)>, IMonitoredRequest
{
    private const string CounterName = "user.sign_in";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}