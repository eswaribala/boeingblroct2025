using Microsoft.EntityFrameworkCore;
using UserAPI.Contexts;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class RoleService : IRoleService
    {
        private UserContext _userContext;

        public RoleService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<Role> AddRole(Role newRole, long UserId)
        {
            var user = await _userContext.Users.FirstAsync(u => u.UserId == UserId);
            newRole.User = user;
            var result = await _userContext.Roles.AddAsync(newRole);
            await _userContext.SaveChangesAsync();
            return result.Entity;
        }

        public Task<bool> DeleteRole(long RoleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleById(long RoleId)
        {
            throw new NotImplementedException();
        }

        public Task<Role> UpdateRole(Role updatedRole)
        {
            throw new NotImplementedException();
        }
    }
}
