using Forum.Domain.Dtos;
using MediatR;

namespace Forum.Domain.UseCases.GetComments;

internal class GetCommentsUseCase(IGetCommentsStorage storage) : IRequestHandler<GetCommentsQuery, (IEnumerable<CommentDto> resources, int totalCount)>
{
    public Task<(IEnumerable<CommentDto> resources, int totalCount)> Handle(GetCommentsQuery request, CancellationToken cancellationToken) =>
        storage.GetComments(request.TopicId, request.Skip, request.Take, cancellationToken);
}