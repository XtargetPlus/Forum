using Forum.Domain.Authentication;

namespace Forum.Domain.Authorization;

public interface IIntentionResolver
{
}

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}