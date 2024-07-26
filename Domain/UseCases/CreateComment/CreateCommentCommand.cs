using Forum.Domain.Dtos;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.CreateComment;

public record CreateCommentCommand(Guid TopicId, string Text) : IRequest<CommentDto>, IMonitoredRequest
{
    private const string CounterName = "comment.created";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}