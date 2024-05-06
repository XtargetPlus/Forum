using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.GetTopics;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class GetTopicsStorage(
        IMapper dataMapper,
        AppDbContext dbContext)
    : IGetTopicsStorage
{
    public async Task<(IEnumerable<TopicDto> resources, int totalCount)> GetTopics(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = dbContext.Topics.Where(t => t.ForumId == query.ForumId);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var resources = await dbQuery
            .AsNoTracking()
            .Skip(query.Skip)
            .Take(query.Take)
            .ProjectTo<TopicDto>(dataMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return (resources, totalCount);
    }
}