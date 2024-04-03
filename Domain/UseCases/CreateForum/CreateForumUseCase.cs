using Domain.Authorization;
using Domain.Dtos;
using MediatR;

namespace Domain.UseCases.CreateForum;

internal class CreateForumUseCase(IIntentionManager intentionManager, ICreateForumStorage storage) : IRequestHandler<CreateForumCommand, ForumDto>
{
    public async Task<ForumDto> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.CreateForum(command, cancellationToken);
    }
}