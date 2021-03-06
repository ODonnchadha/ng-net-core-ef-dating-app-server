﻿using app.api.Extensions;
using AutoMapper;
using System.Linq;

namespace app.api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region DTOs => Entities
            CreateMap<DTOs.MessageForCreation, Entities.Message>();
            CreateMap<DTOs.PhotoForCreation, Entities.Photo>();
            CreateMap<DTOs.UserForRegister, Entities.User>();
            CreateMap<DTOs.UserForUpdate, Entities.User>();
            #endregion

            #region Entities => DTOs
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
            CreateMap<Entities.Message, DTOs.MessageForCreation>();
            CreateMap<Entities.Message, DTOs.MessageToReturn>().ForMember(
                m => m.SenderProfileUrl,
                options => options.MapFrom(
                    u => u.Sender.Photos.FirstOrDefault(p => p.IsDefault).Url)).ForMember(
                m => m.RecipientProfileUrl,
                options => options.MapFrom(
                    u => u.Recipient.Photos.FirstOrDefault(p => p.IsDefault).Url));
            CreateMap<Entities.Photo, DTOs.PhotoForDetails>();
            CreateMap<Entities.Photo, DTOs.PhotoForReturn>();
            #endregion
        }
    }
}
