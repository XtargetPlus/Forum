using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.GetTopics;
using Forum.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class GetTopicsStorage(
        IMapper dataMapper,
        AppDbContext dbContext)
    : IGetTopicsStorage
{
    public async Task<(IEnumerable<TopicDto> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var resources = await dbContext.Database
            .SqlQuery<TopicListItemReadModel>($@"
                SELECT
                    t.""Id"" AS ""TopicId"",
                    t.""ForumId"" AS ""ForumId"",
                    t.""UserId"" AS ""UserId"",
                    t.""Title"" AS ""Title"",
                    t.""CreatedAt"" AS ""CreatedAt"",
                    COALESCE(c.TotalCommentsCount, 0) AS ""TotalCommentsCount"",
                    c.""CreatedAt"" AS ""LastCommentCreatedAt"",
                    c.""Id"" AS ""LastCommentId""
                FROM ""Topics"" AS t
                LEFT JOIN (
                    SELECT
                        ""TopicId"",
                        ""Id"",
                        ""CreatedAt"",
                        COUNT(*) OVER (PARTITION BY ""TopicId"") AS TotalCommentsCount,
                        ROW_NUMBER() OVER (PARTITION BY ""TopicId"" ORDER BY ""CreatedAt"" DESC) RowNumber
                    FROM ""Comments""
                ) AS c ON t.""Id"" = c.""TopicId"" AND c.RowNumber = 1
                WHERE t.""ForumId"" = {forumId}
                ORDER BY
                    COALESCE(c.""CreatedAt"", t.""CreatedAt"") DESC
                LIMIT {take} OFFSET {skip}")
            .ProjectTo<TopicDto>(dataMapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}