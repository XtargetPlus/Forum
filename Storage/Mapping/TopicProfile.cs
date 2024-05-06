using AutoMapper;
using Forum.Domain.Dtos;
using Forum.Storage.Models;

namespace Forum.Storage.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, TopicDto>()
            .ForMember(d => d.TopicId, s => s.MapFrom(t => t.Id));
    }
}