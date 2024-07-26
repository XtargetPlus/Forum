using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using CreateTopicCommand = Forum.Domain.UseCases.CreateTopic.CreateTopicCommand;

namespace Forum.Storage.Storages;

internal class CreateTopicStorage(
        IMapper dataMapper,
        TimeProvider timeProvider,
        AppDbContext dbContext)
    : ICreateTopicStorage
{
    public async Task<TopicDto> CreateTopic(CreateTopicCommand command, Guid userId, CancellationToken cancellationToken)
    {
        var topic = new Topic
        {
            ForumId = command.ForumId,
            UserId = userId,
            Title = command.Title,
            CreatedAt = timeProvider.GetUtcNow()
        };

        await dbContext.Topics.AddAsync(topic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return dataMapper.Map<TopicDto>(topic);
    }
}