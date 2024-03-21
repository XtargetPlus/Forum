using Domain.Authentication;
using Domain.Authorization;
using Domain.Dtos;
using Domain.UseCases.GetForums;
using FluentValidation;

namespace Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        ICreateTopicStorage topicStorage,
        IGetForumsStorage getForumsStorage,
        IIdentityProvider identityProvider)
    : ICreateTopicUseCase
{
    public async Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        // checking rights 
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        await getForumsStorage.ThrowIfNotFound(command.ForumId, cancellationToken);

        return await topicStorage.CreateTopic(command, identityProvider.Current.UserId, cancellationToken);
    }
}