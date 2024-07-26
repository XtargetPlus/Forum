using AutoMapper;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.GetComments;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class GetCommentsStorage(AppDbContext dbContext, IMapper dataMapper) : IGetCommentsStorage
{
    public async Task<(IEnumerable<CommentDto> resources, int totalCount)> GetComments(
        Guid topicId, int skip, int take, CancellationToken cancellationToken)
    {
        var dbQuery = dbContext.Comments.Where(c => c.TopicId == topicId);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var resources = await dbQuery
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ProjectTo<CommentDto>(dataMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return (resources, totalCount);
    }
}