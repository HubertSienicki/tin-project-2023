using AutoMapper;
using UserService.Model;
using UserService.Model.DTOs;

namespace UserService.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserGet>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}