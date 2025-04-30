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
        public T MaxValue;
        public T MinValue;

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (MaxValue != null && MinValue != null)
                {
                    if (value < MinValue && value > MaxValue)
                    {
                       
                    }
                }
                else _value = value;
            }
        }
        public Setting(string name, string device, T maxValue, T minValue)
        {
            Name = name;
            Device = device;
            MaxValue = maxValue;
            MinValue = minValue;
        }
        public Setting(string name, string device, T value) // костыль пока нет ввода с ui
        {
            Name = name;
            Device = device;
            Value = value;
        }
        public Setting(string name, string device)
        {
            Name = name;
            Device = device;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
