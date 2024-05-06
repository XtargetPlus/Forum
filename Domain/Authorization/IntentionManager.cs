using Forum.Domain.Authentication;

namespace Forum.Domain.Authorization;

internal class IntentionManager(
        IEnumerable<IIntentionResolver> resolvers,
        IIdentityProvider identityProvider)
    : IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();

        return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }
}