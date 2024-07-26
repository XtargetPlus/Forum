using Forum.Domain.Authentication;
using Forum.Domain.Authorization.AccessManagement;

namespace Forum.Domain.Authorization;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntention>
{
    public bool IsAllowed(IIdentity subject, AccountIntention intention)
    {
        return intention switch
        {
            AccountIntention.SignOut => subject.IsAuthenticated(),
            _ => false,
        };
    }
}