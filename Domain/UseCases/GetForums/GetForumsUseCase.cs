using Forum.Domain.Dtos;
using MediatR;

namespace Forum.Domain.UseCases.GetForums;

internal class GetForumsUseCase(IGetForumsStorage storage) : IRequestHandler<GetForumsQuery, IEnumerable<ForumDto>>
{
    public Task<IEnumerable<ForumDto>> Handle(GetForumsQuery query, CancellationToken cancellationToken) => storage.GetForums(cancellationToken);
}