using AutoMapper;
using Domain.Dtos;
using Storage.Models;

namespace Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateProjection<User, RecognizedUser>()
            .ForMember(d => d.UserId, s => s.MapFrom(u => u.Id));

        CreateProjection<Session, SessionDto>();
    }
}