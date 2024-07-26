using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Authorization.AccessManagement;
using Forum.Domain.DomainEvents;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.GetForums;
using MediatR;

namespace Forum.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
        IIntentionManager intentionManager,
        IGetForumsStorage getForumsStorage,
        IIdentityProvider identityProvider,
        IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTopicCommand, TopicDto>
{
    public async Task<TopicDto> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        // checking rights 
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        await getForumsStorage.ThrowIfNotFound(command.ForumId, cancellationToken);

        await using var scope = await unitOfWork.StartScope(cancellationToken);

        var createTopicStorage = scope.GetStorage<ICreateTopicStorage>();
        var domainEventStorage = scope.GetStorage<IDomainEventStorage>();

        var topic = await createTopicStorage.CreateTopic(command, identityProvider.Current.UserId, cancellationToken);
        await domainEventStorage.AddEvent(ForumDomainEvent.TopicCreated(topic), cancellationToken);

        await scope.Commit(cancellationToken);

        return topic;
    }
}