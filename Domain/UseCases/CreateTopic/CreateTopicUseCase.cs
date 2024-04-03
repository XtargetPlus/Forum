using Domain.Authentication;
using Domain.Authorization;
using Domain.Dtos;
using Domain.UseCases.GetForums;
using MediatR;

namespace Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
        IIntentionManager intentionManager,
        ICreateTopicStorage topicStorage,
        IGetForumsStorage getForumsStorage,
        IIdentityProvider identityProvider)  
    : IRequestHandler<CreateTopicCommand, TopicDto>
{
    public async Task<TopicDto> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        // checking rights 
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        await getForumsStorage.ThrowIfNotFound(command.ForumId, cancellationToken);

        return await topicStorage.CreateTopic(command, identityProvider.Current.UserId, cancellationToken);
    }
}