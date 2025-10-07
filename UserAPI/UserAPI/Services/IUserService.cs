using UserAPI.Models;

namespace UserAPI.Services
{
    public interface IUserService
    {
        Task<BoeingUser> AddUser(BoeingUser newUser);
        Task<List<BoeingUser>> GetAllUsers();
        Task<BoeingUser> GetUserById(long id);
        Task<BoeingUser> UpdateUser(BoeingUser updatedUser);
        Task<bool> DeleteUser(long id);
    }
}
