using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Authorization.AccessManagement;
using Forum.Domain.DomainEvents;
using Forum.Domain.Dtos;
using Forum.Domain.Exceptions;
using MediatR;

namespace Forum.Domain.UseCases.CreateComment;

internal class CreateCommentUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        await using var scope = await unitOfWork.StartScope(cancellationToken);
        
        var commentStorage = scope.GetStorage<ICreateCommentStorage>();

        var topic = await commentStorage.FindTopic(command.TopicId, cancellationToken);
        if (topic is null)
            throw new TopicNotFoundException(command.TopicId);

        intentionManager.IsAllowed(TopicIntention.CreateComment, topic);

        var comment = await commentStorage.Create(command.TopicId, identityProvider.Current.UserId, command.Text, cancellationToken);

        var domainEventsStorage = scope.GetStorage<IDomainEventStorage>();
        await domainEventsStorage.AddEvent(ForumDomainEvent.CommentCreated(topic, comment), cancellationToken);

        await scope.Commit(cancellationToken);

        return comment;
    }
}