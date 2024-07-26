using AutoMapper;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateForum;
using Microsoft.Extensions.Caching.Memory;

namespace Forum.Storage.Storages;

internal class CreateForumStorage(
        IMapper dataMapper,
        IMemoryCache memoryCache,
        AppDbContext dbContext)
    : ICreateForumStorage
{
    public async Task<ForumDto> CreateForum(CreateForumCommand command, CancellationToken cancellationToken)
    {
        var forum = new Entities.Forum
        {
            Title = command.Title,
        };

        await dbContext.Forums.AddAsync(forum, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(nameof(GetForumsStorage.GetForums));

        return dataMapper.Map<ForumDto>(forum);
    }
}