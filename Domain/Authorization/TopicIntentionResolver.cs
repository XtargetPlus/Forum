using Forum.Domain.Authentication;
using Forum.Domain.Authorization.AccessManagement;
using Forum.Domain.Dtos;

namespace Forum.Domain.Authorization;

internal class TopicIntentionResolver : 
    IIntentionResolver<TopicIntention>,
    IIntentionResolver<TopicIntention, TopicDto>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthenticated(),
        _ => false
    };

    public bool IsAllowed(IIdentity subject, TopicIntention intention, TopicDto target) => intention switch
    {
        TopicIntention.CreateComment => subject.IsAuthenticated(),
        _ => false
    };
}