namespace Forum.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
}