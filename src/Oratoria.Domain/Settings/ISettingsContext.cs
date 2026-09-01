using System.Numerics;

namespace Oratoria.Domain.Settings
{
    public interface ISettingsContext
    {
        Setting<T> GetSetting<T>(
            Enum deviceId, string key, string displayName, string unit,
            T defaultValue, T? minValue = default, T? maxValue = default) where T : struct, INumber<T>;

        IReadOnlyList<Setting> All { get; }
    }
}
