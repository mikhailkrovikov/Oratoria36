using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence
{
    public class AppDBContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        public AppDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.db");
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
