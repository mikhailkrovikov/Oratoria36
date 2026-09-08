using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence
{
    public class SettingDBContext : DbContext
    {
        public DbSet<DeviceSettingEntity> DeviceSettings { get; set; }
        public SettingDBContext(DbContextOptions<SettingDBContext> options) : base(options)
        {
        }
    }
}
