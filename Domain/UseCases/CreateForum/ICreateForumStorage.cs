using Domain.Dtos;

namespace Domain.UseCases.CreateForum;

public interface ICreateForumStorage
{
    Task<ForumDto> CreateForum(CreateForumCommand command, CancellationToken cancellationToken);
}