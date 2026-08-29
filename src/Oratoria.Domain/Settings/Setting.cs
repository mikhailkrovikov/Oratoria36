using Oratoria.Domain.Devices;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Oratoria.Domain.Settings
{
    public class Setting<T> : Setting where T : INumber<T>
    {
        private T? _maxValue;
        private T? _minValue;
        private T _value;

        public T? MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public T? MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                bool hasMinConstraint = _minValue != null;
                bool hasMaxConstraint = _maxValue != null;

                T newValue = value;

                if (hasMinConstraint && value.CompareTo(_minValue) < 0)
                {
                    newValue = _minValue;
                }
                else if (hasMaxConstraint && value.CompareTo(_maxValue) > 0)
                {
                    newValue = _maxValue;
                }

                if (!EqualityComparer<T>.Default.Equals(_value, newValue))
                {
                    _value = newValue;
                    OnPropertyChanged();
                }
            }
        }

        [JsonConstructor]
        public Setting(Enum deviceId, string name, string unit) : base(deviceId, name, unit)
        {
        }

        public Setting(Enum deviceId, string name, string unit,  T value) : base(deviceId, name, unit)
        {
            _value = value;
        }

        public Setting(Enum deviceId, string name, string unit, T value, T? minValue, T? maxValue) : base(deviceId, name, unit)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            if (minValue != null && value.CompareTo(minValue) < 0)
            {
                _value = minValue;
            }
            else if (maxValue != null && value.CompareTo(maxValue) > 0)
            {
                _value = maxValue;
            }
            else
            {
                _value = value;
            }
        }


    }

    public class Setting : INotifyPropertyChanged
    {
        public readonly string Name;
        public readonly Enum DeviceId;
        public readonly string Unit;

        public Setting(Enum deviceId, string name, string unit)
        {
            Name = name;
            Unit = unit;
            DeviceId = deviceId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
