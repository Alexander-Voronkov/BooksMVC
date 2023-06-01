using BooksMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksMVC.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users => Set<User>();
    }
}
