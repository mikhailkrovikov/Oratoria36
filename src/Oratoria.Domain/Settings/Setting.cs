using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oratoria.Domain.Settings
{
    public class Setting : INotifyPropertyChanged
    {
        public readonly string Key;
        public readonly string Name;
        public readonly Enum DeviceId;
        public readonly string Unit;

        public Setting(Enum deviceId, string name, string unit) : this(deviceId, name, name, unit)
        {
        }

        public Setting(Enum deviceId, string key, string name, string unit)
        {
            Key = key;
            Name = name;
            Unit = unit;
            DeviceId = deviceId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
