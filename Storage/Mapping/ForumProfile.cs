﻿using AutoMapper;
using Forum.Domain.Dtos;

namespace Forum.Storage.Mapping;

public class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Models.Forum, ForumDto>()
            .ForMember(d => d.ForumId, s => s.MapFrom(f => f.Id));
    }
}