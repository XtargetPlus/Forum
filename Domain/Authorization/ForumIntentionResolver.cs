using Forum.Domain.Authentication;
using Forum.Domain.Authorization.AccessManagement;

namespace Forum.Domain.Authorization;

internal class ForumIntentionResolver : IIntentionResolver<ForumIntention>
{
    public bool IsAllowed(IIdentity subject, ForumIntention intention) => intention switch
    {
        ForumIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}