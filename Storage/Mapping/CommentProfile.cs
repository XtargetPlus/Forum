using AutoMapper;
using Forum.Domain.Dtos;
using Forum.Storage.Entities;

namespace Forum.Storage.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(
                d => d.CommentId, 
                s => s.MapFrom(c => c.Id));
    }
}