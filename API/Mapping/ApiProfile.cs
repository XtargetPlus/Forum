using API.Dtos.Responses;
using AutoMapper;
using Domain.Dtos;

namespace API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<ForumDto, ForumResponse>();
        CreateMap<TopicDto, TopicResponse>();
    }
}