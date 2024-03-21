using Domain.Dtos;
using Domain.UseCases.GetForums;
using FluentValidation;

namespace Domain.UseCases.GetTopics;

internal class GetTopicsUseCase(
        IGetTopicsStorage topicsStorage,
        IGetForumsStorage forumsStorage,
        IValidator<GetTopicsQuery> validator)
    : IGetTopicsUseCase
{
    public async Task<(IEnumerable<TopicDto> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        await forumsStorage.ThrowIfNotFound(query.ForumId, cancellationToken);
        return await topicsStorage.GetTopics(query, cancellationToken);
    }
}