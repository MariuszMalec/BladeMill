using AutoMapper;
using BladeMill.BLL.DAL;
using BladeMill.BLL.Models;

namespace BladeMill.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()//TODO musi byc w Web aby dzialalo to!
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.FullName, o => o.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                .ForMember(d => d.Sso, o => o.Ignore())
                ;

            CreateMap<UserDto, User>();

            CreateMap<UserDto, BMUserDto>()
                .ForMember(d => d.FullName, o => o.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                ;

            ;
        }
    }
}
