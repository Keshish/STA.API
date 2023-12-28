using AutoMapper;
using STA.API.Dtos.Parent;
using STA.API.Models.Users;
using STA.API.ViewModels;

namespace STA.API.Profiles
{
    public class ParentProfile : Profile
    {
        public ParentProfile()
        {
        
            CreateMap<Parent, ParentVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
