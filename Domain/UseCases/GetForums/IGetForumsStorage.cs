using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken);
}