using Domain.Dtos;

namespace Domain.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
}