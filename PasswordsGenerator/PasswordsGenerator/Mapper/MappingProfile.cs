using AutoMapper;
using DataLayer.Entities;
using PasswordsGenerator.Dtos;
using PasswordsGenerator.Models;
using PasswordsGenerator.ViewModels.Home;

namespace PasswordsGenerator.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<UserPasswordGenerated, UserPasswordGeneratedDto>()
                .ForMember(dest => dest.userID, map => map.MapFrom(src => src.UserID))
                .ForMember(dest => dest.passwordGenerationDatetime, map => map.MapFrom(src => src.PasswordGenerationDatetime))
                .ForMember(dest => dest.generatedPassword, map => map.MapFrom(src => src.GeneratedPassword))
                .ReverseMap();

            CreateMap<UserPasswordGeneratedDto, UserPasswordGeneratedModel>()
                .ForMember(dest => dest.userID, map => map.MapFrom(src => src.userID))
                .ForMember(dest => dest.passwordGenerationDatetime, map => map.MapFrom(src => src.passwordGenerationDatetime))
                .ForMember(dest => dest.generatedPassword, map => map.MapFrom(src => src.generatedPassword))
                .ReverseMap();

            CreateMap<UserPasswordGeneratedDto, HomeViewModel>()
                .ForMember(dest => dest.userID, map => map.MapFrom(src => src.userID))
                .ForMember(dest => dest.passwordGenerationDatetime, map => map.MapFrom(src => src.passwordGenerationDatetime))
                .ReverseMap();

            CreateMap<UserPasswordGeneratedDto, GeneratedPasswordViewModel>()
                .ForMember(dest => dest.userID, map => map.MapFrom(src => src.userID))
                .ForMember(dest => dest.passwordGenerationDatetime, map => map.MapFrom(src => src.passwordGenerationDatetime))
                .ForMember(dest => dest.generatedPassword, map => map.MapFrom(src => src.generatedPassword))
                .ReverseMap();
        }
    }
}
