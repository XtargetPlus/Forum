using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateComment;
using Forum.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class CreateCommentStorage(
    IMapper dataMapper,
    AppDbContext dbContext, 
    TimeProvider timeProvider) : ICreateCommentStorage
{
    public Task<TopicDto?> FindTopic(Guid topicId, CancellationToken cancellationToken) => dbContext.Topics
        .Where(t => t.Id == topicId)
        .ProjectTo<TopicDto>(dataMapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<CommentDto> Create(Guid topicId, Guid userId, string text, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            TopicId = topicId,
            CreatedAt = timeProvider.GetUtcNow(),
            UserId = userId,
            Text = text,
        };
        
        await dbContext.Comments.AddAsync(comment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Comments
            .Where(c => c.Id == comment.Id)
            .ProjectTo<CommentDto>(dataMapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}