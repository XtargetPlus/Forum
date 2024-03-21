using Domain.Dtos;

namespace Domain.UseCases.CreateForum;

public interface ICreateForumUseCase
{
    Task<ForumDto> Execute(CreateForumCommand command, CancellationToken cancellationToken);
}