using Domain.Dtos;

namespace Domain.UseCases.GetTopics;

public interface IGetTopicsUseCase
{
    Task<(IEnumerable<TopicDto> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken);
}