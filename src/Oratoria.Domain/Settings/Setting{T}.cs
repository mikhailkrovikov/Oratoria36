using System.Numerics;

namespace Oratoria.Domain.Settings
{
    public class Setting<T> : Setting where T : struct, INumber<T>
    {
        private T? _maxValue;
        private T? _minValue;
        private T _value;

        public T? MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        public T? MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged();
            }
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_minValue != null && value < _minValue.Value)
                    value = _minValue.Value;
                else if (_maxValue != null && value > _maxValue.Value)
                    value = _maxValue.Value;

                _value = value;
                OnPropertyChanged();
            }
        }

        public Setting(Enum deviceId, string name, string unit) : base(deviceId, name, unit)
        {
        }

        public Setting(Enum deviceId, string key, string name, string unit, T value, T? minValue, T? maxValue)
            : base(deviceId, key, name, unit)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _value = value;
        }
    }
}
