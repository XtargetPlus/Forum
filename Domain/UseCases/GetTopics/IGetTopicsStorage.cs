using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<TopicDto> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken);
}