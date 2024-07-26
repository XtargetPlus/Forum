using AutoMapper;
using Forum.Domain.DomainEvents;

namespace Forum.Storage.Mapping;

public class DomainEventsProfile : Profile
{
    public DomainEventsProfile()
    {
        CreateMap<ForumDomainEvent, Models.ForumDomainEvent>();
        CreateMap<ForumDomainEvent.ForumComment, Models.ForumDomainEvent.ForumComment>();
    }
}