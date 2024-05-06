using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.CreateForum;

public interface ICreateForumStorage : IStorage
{
    Task<ForumDto> CreateForum(CreateForumCommand command, CancellationToken cancellationToken);
}