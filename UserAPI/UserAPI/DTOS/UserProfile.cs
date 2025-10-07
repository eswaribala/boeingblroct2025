using AutoMapper;
using UserAPI.Models;

namespace UserAPI.DTOS
{
    public class UserProfile:Profile
    {
        // Entity -> DTO
        CreateMap<BoeingUser, BoeingUserReadDTO>();

            // DTO -> Entity
        CreateMap<VehicleDTO, Vehicle>();
    }
}
