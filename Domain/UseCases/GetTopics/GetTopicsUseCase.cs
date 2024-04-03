using Domain.Dtos;
using Domain.UseCases.GetForums;
using MediatR;

namespace Domain.UseCases.GetTopics;

internal class GetTopicsUseCase(
        IGetTopicsStorage topicsStorage,
        IGetForumsStorage forumsStorage)
    : IRequestHandler<GetTopicsQuery, (IEnumerable<TopicDto> resources, int totalCount)>
{
    public async Task<(IEnumerable<TopicDto> resources, int totalCount)> Handle(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await forumsStorage.ThrowIfNotFound(query.ForumId, cancellationToken);
        return await topicsStorage.GetTopics(query, cancellationToken);
    }
}