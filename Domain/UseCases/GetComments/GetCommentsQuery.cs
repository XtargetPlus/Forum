using Forum.Domain.Dtos;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.GetComments;

public record GetCommentsQuery(Guid TopicId, int Skip, int Take) : IRequest<(IEnumerable<CommentDto> resources, int totalCount)>, IMonitoredRequest
{
    private const string CounterName = "comments.fetched";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}