using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.CreateTopic;

public interface ICreateTopicStorage : IStorage
{
    Task<TopicDto> CreateTopic(CreateTopicCommand command, Guid userId, CancellationToken cancellationToken);
}