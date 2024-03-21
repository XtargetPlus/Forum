using Domain.Authentication;
using Domain.Authorization;

namespace Domain.UseCases.SignOut;

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