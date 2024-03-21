using AutoMapper;
using Domain.Dtos;
using Storage.Models;

namespace Storage.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, TopicDto>()
            .ForMember(d => d.TopicId, s => s.MapFrom(t => t.Id));
    }
}