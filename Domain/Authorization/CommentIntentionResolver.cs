using Forum.Domain.Authentication;
using Forum.Domain.Authorization.AccessManagement;

namespace Forum.Domain.Authorization;

internal class CommentIntentionResolver : IIntentionResolver<CommentIntention>
{
    public bool IsAllowed(IIdentity subject, CommentIntention intention) => intention switch
    {
        _ => false
    };
}