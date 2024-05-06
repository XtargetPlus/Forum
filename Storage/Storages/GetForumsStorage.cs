using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.GetForums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Forum.Storage.Storages;

internal class GetForumsStorage(
        IMapper dataMapper,
        IMemoryCache memoryCache,
        AppDbContext dbContext)
    : IGetForumsStorage
{
    public async Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken) => (await memoryCache.GetOrCreateAsync(
        nameof(GetForums),
        entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
            return dbContext.Forums
                .ProjectTo<ForumDto>(dataMapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
        }))!;
}