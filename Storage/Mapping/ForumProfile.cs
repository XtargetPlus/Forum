using AutoMapper;
using Domain.Dtos;
using Storage.Models;

namespace Storage.Mapping;

public class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Forum, ForumDto>()
            .ForMember(d => d.ForumId, s => s.MapFrom(f => f.Id));
    }
}