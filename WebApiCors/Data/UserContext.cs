using Microsoft.EntityFrameworkCore;
using WebApiCors.Models;

namespace WebApiCors.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(prop => prop.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });
                            
        }
    }
}
