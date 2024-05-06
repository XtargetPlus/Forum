using Forum.Domain.Authentication;
using Forum.Domain.Authorization;

namespace Forum.Domain.UseCases.SignOut;

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