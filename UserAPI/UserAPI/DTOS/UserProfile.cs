using AutoMapper;
using UserAPI.Models;

namespace UserAPI.DTOS
{
    public class UserProfile:Profile
    {
        public UserProfile() {
            // Entity -> DTO
            CreateMap<FullName, FullNameDTO>();
            CreateMap<BoeingUser, BoeingUserReadDTO>();

            // DTO -> Entity
            CreateMap<FullNameDTO, FullName>();
            CreateMap<BoeingUserDTO,BoeingUser>();
        }
       
    }
}
