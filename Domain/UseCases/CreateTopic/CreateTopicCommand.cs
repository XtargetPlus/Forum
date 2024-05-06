using Forum.Domain.Dtos;
using Forum.Domain.Monitoring;
using MediatR;

namespace Forum.Domain.UseCases.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<TopicDto>, IMonitoredRequest
{
    private const string CounterName = "topics.created";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}