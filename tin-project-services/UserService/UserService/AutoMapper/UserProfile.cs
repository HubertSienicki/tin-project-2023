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
        CreateMap<RegisterModel, UserPost>() // role_id defaults to 2 (User)
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2));
    }
}