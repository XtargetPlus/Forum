using Forum.Domain.Authorization;
using Forum.Domain.Dtos;
using MediatR;

namespace Forum.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(IIntentionManager intentionManager, ICreateForumStorage storage) : IRequestHandler<CreateForumCommand, ForumDto>
{
    public async Task<ForumDto> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.CreateForum(command, cancellationToken);
    }
}