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

        public async Task<Role> AddRole(Role newRole)
        {
            var user = await _userContext.Users.FirstAsync(u => u.UserId == newRole.UserId);
            newRole.User = user;
            var result = await _userContext.Roles.AddAsync(newRole);
            await _userContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteRole(long RoleId)
        {
            var role = await _userContext.Roles.FirstOrDefaultAsync(r => r.RoleId == RoleId);
            if (role != null)
            {
                _userContext.Roles.Remove(role);
               await _userContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Role>> GetAllRoles()
        {
           return await _userContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(long RoleId)
        {
            var role =  await _userContext.Roles.FirstOrDefaultAsync(r => r.RoleId == RoleId);
            if (role != null)
            {
                return role;
            }
            return null;
        }

        public async Task<Role> UpdateRole(Role updatedRole)
        {
            var user = await _userContext.Users.FirstAsync(u => u.UserId == updatedRole.UserId);
            updatedRole.User = user;
            var role = await _userContext.Roles.FirstOrDefaultAsync(r => r.RoleId == updatedRole.RoleId);
            if (role != null)
            {
                _userContext.Entry(role).CurrentValues.SetValues(updatedRole);
                await _userContext.SaveChangesAsync();
                return updatedRole;
            }
            return null;
        }
    }
}
