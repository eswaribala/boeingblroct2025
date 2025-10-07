using AutoMapper;
using UserAPI.Models;

namespace UserAPI.DTOS
{
    public class UserProfile:Profile
    {
        public UserProfile() {
            // Entity -> DTO
            CreateMap<BoeingUser, BoeingUserReadDTO>();

            // DTO -> Entity
            CreateMap<BoeingUserDTO,BoeingUser>();
        }
       
    }
}
