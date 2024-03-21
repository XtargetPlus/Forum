using Domain.Exceptions;

namespace Domain.UseCases.GetForums;

internal static class GetForumStorageExtensions
{
    private static async Task<bool> ForumExists(this IGetForumsStorage storage, Guid forumId, CancellationToken cancellationToken)
    {
        var forums = await storage.GetForums(cancellationToken);
        return forums.Any(f => f.ForumId == forumId);
    }

    public static async Task ThrowIfNotFound(this IGetForumsStorage storage, Guid forumId, CancellationToken cancellationToken)
    {
        if (!await ForumExists(storage, forumId, cancellationToken))
        {
            throw new ForumNotFoundException(forumId);
        }
    }
}