using Domain.Authorization;
using Domain.Dtos;
using FluentValidation;
using MediatR;

namespace Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
        IValidator<CreateForumCommand> validator,
        IIntentionManager intentionManager,
        ICreateForumStorage storage)
    : IRequestHandler<CreateForumCommand, ForumDto>
{
    public async Task<ForumDto> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.CreateForum(command, cancellationToken);
    }
}