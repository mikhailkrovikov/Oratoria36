using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.Services
{
    public interface ISettingsService
    {
        IReadOnlyList<DeviceSettingEntity> Load();
        void Upsert(DeviceSettingEntity entity);
    }
}
