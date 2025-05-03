using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Settings
{
    public class Setting<T> : INotifyPropertyChanged where T : INumber<T>
    {
        readonly string Name;
        readonly string Device;

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

        public Setting(string name, string device, T value)
        {
            Name = name;
            Device = device;
            _value = value;
        }

        public Setting(string name, string device, T value, T? minValue, T? maxValue)
        {
            Name = name;
            Device = device;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
