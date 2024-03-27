using Domain.Dtos;
using Domain.Monitoring;
using MediatR;

namespace Domain.UseCases.GetForums;

public record GetForumsQuery : IRequest<IEnumerable<ForumDto>>, IMonitoredRequest
{
    private const string CounterName = "forums_fetched";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}