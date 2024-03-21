using Domain.Dtos;

namespace Domain.UseCases.GetForums;

public interface IGetForumsUseCase
{
    Task<IEnumerable<ForumDto>> Execute(CancellationToken cancellationToken);
}