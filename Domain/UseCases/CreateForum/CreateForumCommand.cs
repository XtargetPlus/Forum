using Domain.Dtos;
using Domain.Monitoring;
using MediatR;

namespace Domain.UseCases.CreateForum;

public record CreateForumCommand(string Title) : IRequest<ForumDto>, IMonitoredRequest
{
    private const string CounterName = "forums.created";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}