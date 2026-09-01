using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(DeviceSettingConfiguration))]
    public class DeviceSettingEntity
    {
        public string DeviceKey { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string? Min { get; set; }
        public string? Max { get; set; }
    }
}
