using Domain.Dtos;

namespace Domain.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    Task<TopicDto> CreateTopic(CreateTopicCommand command, Guid userId, CancellationToken cancellationToken); 
}