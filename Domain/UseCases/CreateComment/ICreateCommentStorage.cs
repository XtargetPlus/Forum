using Forum.Domain.Dtos;

namespace Forum.Domain.UseCases.CreateComment;

public interface ICreateCommentStorage : IStorage
{
    Task<TopicDto?> FindTopic(Guid topicId, CancellationToken cancellationToken);
    Task<CommentDto> Create(Guid topicId, Guid userId, string text, CancellationToken cancellationToken);
}