using Domain.Authentication;
using Domain.Authorization;

namespace Domain.UseCases.CreateTopic;

internal class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}