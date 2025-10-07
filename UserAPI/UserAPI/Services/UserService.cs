using Microsoft.EntityFrameworkCore;
using UserAPI.Contexts;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class UserService : IUserService
    {
        private UserContext _userContext;

        public UserService(UserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<BoeingUser> AddUser(BoeingUser newUser)
        {
           var result= await _userContext.Users.AddAsync(newUser);
            await _userContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<bool> DeleteUser(long id)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<BoeingUser>> GetAllUsers()
        {
           return await _userContext.Users.ToListAsync();
        }

        public async Task<BoeingUser> GetUserById(long id)
        {
            var user= await _userContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if(user!=null)
            {
                return user;
            }
            return null;
        }

        public async Task<BoeingUser> UpdateUser(BoeingUser updatedUser)
        {
            var user = _userContext.Users.FirstOrDefaultAsync(u => u.UserId == updatedUser.UserId);
            if (user != null)
            {
                _userContext.Entry(user).CurrentValues.SetValues(updatedUser);
                await _userContext.SaveChangesAsync();
                return updatedUser;
            }
            return null;
        }
    }
}
