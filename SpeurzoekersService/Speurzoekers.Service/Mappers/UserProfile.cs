using AutoMapper;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Service.Dto;

namespace Speurzoekers.Service.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AuthenticateDto, UserRegister>().ForMember(dest => dest.EmailAddress,
                opt => opt.MapFrom(s => s.UserName) );

            CreateMap<UserRegister, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(dest=> dest.UserName));
        }
    }
}
