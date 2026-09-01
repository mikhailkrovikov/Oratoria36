using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class DeviceSettingConfiguration : IEntityTypeConfiguration<DeviceSettingEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceSettingEntity> builder)
        {
            builder.HasKey(s => new { s.DeviceKey, s.Name });

            builder.Property(s => s.DeviceKey).IsRequired();
            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.Value).IsRequired();
            builder.Property(s => s.Min);
            builder.Property(s => s.Max);
        }
    }
}
