using System;

namespace Oratoria36.Models.Signals
{
    public class OutputSignal : Signal
    {
        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    ValueChanged?.Invoke(value);
                    Service.ModbusPoller.Instance.WriteSignalAsync(this);
                }
            }
        }

        public event Action<bool> ValueChanged;
        public OutputSignal(string name, ushort channel) : base(name, channel) { }

        internal void UpdateValueWithoutWrite(bool newValue)
        {
            if (_value != newValue)
            {
                _value = newValue;
                OnPropertyChanged(nameof(Value));
                ValueChanged?.Invoke(newValue);
            }
        }
    }
}
