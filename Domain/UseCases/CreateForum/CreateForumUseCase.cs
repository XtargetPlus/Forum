using Domain.Authorization;
using Domain.Dtos;
using FluentValidation;

namespace Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
        IValidator<CreateForumCommand> validator,
        IIntentionManager intentionManager,
        ICreateForumStorage storage)
    : ICreateForumUseCase
{
    public async Task<ForumDto> Execute(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.CreateForum(command, cancellationToken);
    }
}