using AutoMapper;
using Vita_WebApi_API.Dto;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Video, VideoDto>().ReverseMap();
        CreateMap<Activity, ActivityDto>()
            .ReverseMap();
    }
}