using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence
{
    public class SettingDBContext : DbContext
    {
        public DbSet<DeviceSettingEntity> DeviceSettings { get; set; }
        public SettingDBContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.db");
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
