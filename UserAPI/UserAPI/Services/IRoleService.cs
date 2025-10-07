using UserAPI.Models;

namespace UserAPI.Services
{
    public interface IRoleService
    {
        Task<Role> AddRole(Role newRole, long UserId);
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(long RoleId);
        Task<Role> UpdateRole(Role updatedRole, long UserId);
        Task<bool> DeleteRole(long RoleId);
    }
}
