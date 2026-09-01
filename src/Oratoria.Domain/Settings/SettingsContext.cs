using Oratoria.Persistence.Entities;
using Oratoria.Persistence.Services;
using System.Globalization;
using System.Numerics;

namespace Oratoria.Domain.Settings
{
    public class SettingsContext : ISettingsContext
    {
        private readonly ISettingsService _settingsService;
        private readonly Dictionary<(string DeviceKey, string Name), DeviceSettingEntity> _records;
        private readonly List<Setting> _all = new();

        public IReadOnlyList<Setting> All => _all;

        public SettingsContext(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _records = settingsService.Load().ToDictionary(r => (r.DeviceKey, r.Name));
        }

        public Setting<T> GetSetting<T>(
            Enum deviceId, string key, string displayName, string unit,
            T defaultValue, T? minValue = default, T? maxValue = default) where T : struct, INumber<T>
        {
            var deviceKey = $"{deviceId.GetType().FullName}.{deviceId}";
            var value = defaultValue;
            var min = minValue;
            var max = maxValue;

            if (_records.TryGetValue((deviceKey, key), out var record))
            {
                value = Parse(record.Value, defaultValue);
                min = ParseBound(record.Min, minValue);
                max = ParseBound(record.Max, maxValue);
            }

            var setting = new Setting<T>(deviceId, key, displayName, unit, value, min, max);

            if (!_records.ContainsKey((deviceKey, key)))
                Persist(setting);

            setting.PropertyChanged += (_, _) => Persist(setting);
            _all.Add(setting);
            return setting;
        }

        private void Persist<T>(Setting<T> setting) where T : struct, INumber<T>
        {
            _settingsService.Upsert(new DeviceSettingEntity
            {
                DeviceKey = $"{setting.DeviceId.GetType().FullName}.{setting.DeviceId}",
                Name = setting.Key,
                Value = setting.Value.ToString(null, CultureInfo.InvariantCulture),
                Min = setting.MinValue?.ToString(null, CultureInfo.InvariantCulture),
                Max = setting.MaxValue?.ToString(null, CultureInfo.InvariantCulture)
            });
        }

        private static T Parse<T>(string? raw, T fallback) where T : struct, INumber<T>
        {
            if (T.TryParse(raw, CultureInfo.InvariantCulture, out var parsed))
                return parsed;
            return fallback;
        }

        private static T? ParseBound<T>(string? raw, T? fallback) where T : struct, INumber<T>
        {
            if (raw is null)
                return null;
            if (T.TryParse(raw, CultureInfo.InvariantCulture, out var parsed))
                return parsed;
            return fallback;
        }
    }
}
