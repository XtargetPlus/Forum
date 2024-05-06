using Forum.Domain.Dtos;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.GetForums;

public record GetForumsQuery : IRequest<IEnumerable<ForumDto>>, IMonitoredRequest
{
    private const string CounterName = "forums.fetched";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}