using System;

namespace Oratoria36.Models.Signals
{
    public class InputSignal : Signal
    {
        private bool _value;
        public bool Value
        {
            get => _value;
            internal set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    ValueChanged?.Invoke(value);
                }
            }
        }

        public event Action<bool> ValueChanged;

        public InputSignal(string name, ushort channel) : base(name, channel)
        {
        }
    }
}
