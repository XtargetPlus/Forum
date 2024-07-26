using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.GetComments;

public interface IGetCommentsStorage
{
    Task<(IEnumerable<CommentDto> resources, int totalCount)> GetComments(
        Guid topicId, int skip, int take, CancellationToken cancellationToken);
}