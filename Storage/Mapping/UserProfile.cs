using AutoMapper;
using Forum.Domain.Dtos;
using Forum.Storage.Entities;

namespace Forum.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateProjection<User, RecognizedUser>()
            .ForMember(d => d.UserId, s => s.MapFrom(u => u.Id));

        CreateProjection<Session, SessionDto>();
    }
}