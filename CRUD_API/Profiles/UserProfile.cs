using AutoMapper;
using CRUD.Model.Models;
using CRUD.Model.ModelsDTO;

namespace CRUD_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User1, UserDTO>()
                .ForMember(dest =>
                           dest.FName,
                           opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest =>
                           dest.LName,
                           opt => opt.MapFrom(src => src.LastName))
                .ReverseMap();
        }
    }
}
