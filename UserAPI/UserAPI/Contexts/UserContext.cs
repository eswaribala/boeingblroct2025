using Microsoft.EntityFrameworkCore;

using UserAPI.Models;

namespace UserAPI.Contexts
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
        }
        public DbSet<BoeingUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
