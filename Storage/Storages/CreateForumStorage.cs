using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateForum;
using Microsoft.EntityFrameworkCore;
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
        var forum = new Models.Forum
        {
            Title = command.Title,
        };
        await dbContext.Forums.AddAsync(forum, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(nameof(GetForumsStorage.GetForums));

        return await dbContext.Forums
            .Where(f => f.Id == forum.Id)
            .ProjectTo<ForumDto>(dataMapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}