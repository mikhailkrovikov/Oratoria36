using System.Numerics;

namespace Oratoria.Domain.Settings
{
    public class SettingsContext : ISettingsContext
    {
        private readonly List<Setting> _all = new();

        public IReadOnlyList<Setting> All => _all;

        public Setting<T> GetSetting<T>(
            Enum deviceId, string key, string displayName, string unit,
            T defaultValue, T? minValue = default, T? maxValue = default) where T : struct, INumber<T>
        {
            var setting = new Setting<T>(deviceId, key, displayName, unit, defaultValue, minValue, maxValue);
            _all.Add(setting);
            return setting;
        }
    }
}
