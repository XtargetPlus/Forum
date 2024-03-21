using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Dtos;
using Domain.UseCases.CreateTopic;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using CreateTopicCommand = Domain.Dtos.CreateTopicCommand;

namespace Storage.Storages;

internal class CreateTopicStorage(
        IMapper dataMapper,
        IMomentProvider momentProvider,
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
            CreatedAt = momentProvider.Now
        };

        await dbContext.Topics.AddAsync(topic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Topics
            .Where(t => t.Id == topic.Id)
            .ProjectTo<TopicDto>(dataMapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}