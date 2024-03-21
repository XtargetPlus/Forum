using Domain.Dtos;

namespace Domain.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken);
}