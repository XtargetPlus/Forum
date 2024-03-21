using Domain.Dtos;

namespace Domain.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<TopicDto> resources, int totalCount)> GetTopics(GetTopicsQuery query, CancellationToken cancellationToken);
}