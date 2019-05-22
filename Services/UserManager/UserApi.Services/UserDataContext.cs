using Microsoft.EntityFrameworkCore;
namespace UserApi.Services
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}