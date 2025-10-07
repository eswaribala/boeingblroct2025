using AutoMapper;

namespace UserAPI.DTOS
{
    public class RoleProfile:Profile
    {
        public RoleProfile() {
            CreateMap<Models.Role, RoleDTO>().ReverseMap();
            CreateMap<Models.Role, RoleReadDTO>();
        }
    }
}
