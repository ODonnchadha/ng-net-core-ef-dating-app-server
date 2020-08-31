﻿using app.api.Extensions;
using AutoMapper;
using System.Linq;

namespace app.api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Entities.User, DTOs.UserForList>()
                .ForMember(
                destination => destination.PhotoUrl,
                options => options.MapFrom(
                    source => source.Photos.FirstOrDefault(
                        photo => photo.IsDefault).Url))
                .ForMember(destination => destination.Age,
                options => options.MapFrom(
                    source => source.DateOfBirth.Age()));

            CreateMap<Entities.User, DTOs.UserForDetails>()
                .ForMember(
                destination => destination.PhotoUrl,
                options => options.MapFrom(
                    source => source.Photos.FirstOrDefault(
                        photo => photo.IsDefault).Url))
                .ForMember(destination => destination.Age,
                options => options.MapFrom(
                    source => source.DateOfBirth.Age()));

            CreateMap<Entities.Photo, DTOs.PhotoForDetails>();
        }
    }
}
