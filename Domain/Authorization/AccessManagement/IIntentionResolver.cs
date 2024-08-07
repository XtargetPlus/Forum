﻿using Forum.Domain.Authentication;

namespace Forum.Domain.Authorization.AccessManagement;

public interface IIntentionResolver;

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}

public interface IIntentionResolver<in TIntention, in TObject> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention, TObject target);
}