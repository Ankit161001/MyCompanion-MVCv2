using Microsoft.EntityFrameworkCore;
using MyCompanion.Models.Domain;

namespace MyCompanion.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}
