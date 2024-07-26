using AutoMapper;
using Forum.API.Models.Responses;
using Forum.Domain.Dtos;

namespace Forum.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<ForumDto, ForumResponse>();
        CreateMap<TopicDto, TopicResponse>();
        CreateMap<CommentDto, CommentResponse>();
    }
}