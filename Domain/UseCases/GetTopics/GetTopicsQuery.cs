﻿using Domain.Dtos;
using Domain.Monitoring;
using MediatR;

namespace Domain.UseCases.GetTopics;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take) : IRequest<(IEnumerable<TopicDto> resources, int totalCount)>, IMonitoredRequest
{
    private const string CounterName = "topics_fetched";

    public void MonitorSuccess(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => metrics.IncrementCount(CounterName, 1, DomainMetrics.ResultTags(false));
}