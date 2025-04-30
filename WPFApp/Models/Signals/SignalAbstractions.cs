using Modbus.Device;
using NLog;
using Oratoria36.Models.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Signals
{
    public abstract class Signal
    {
        public string Name { get; set; }
        public ushort PinNumber { get; set; }

        protected NetConfig _netConfig;
        public ModbusIpMaster _master => _netConfig.Master;

        public Signal(string name, ushort pinNumber, NetConfig netConfig)
        {
            Name = name;
            PinNumber = pinNumber;
            _netConfig = netConfig;
        }
    }

    public abstract class Signal<T> : Signal
    {
        protected T _value;
        public event Action<T> SignalChanged;
        public Signal(string name, ushort pinNumber, NetConfig netConfig) : base(name, pinNumber, netConfig) { }

        public void OnSignalChanged(T value) => SignalChanged?.Invoke(value);
    }

    public abstract class InputSignalX<T> : Signal<T>, IInputSignal<T>
    {
        private static readonly Logger _logger = LogManager.GetLogger("InputSignal");

        public T Value
        {
            get
            {
                if (_master == null)
                    return _value;
                else
                {
                    Value = GetInput(PinNumber);
                    return _value;
                }
            }
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    OnSignalChanged(_value);

                    if (oldValue.GetType() == typeof(bool) && _value.GetType() == typeof(bool))
                        _logger.Info($"{Name}; Пин {PinNumber} изменил значение с {oldValue} на {_value}");
                }
            }
        }

        public InputSignalX(string name, ushort pinNumber, NetConfig netConfig) : base(name, pinNumber, netConfig) { }

        public T GetInput(ushort pinNumber)
        {
            try
            {
                if (_master != null)
                {
                    var result = Strategy(pinNumber, 1);
                    return result[0];
                }
                else
                {
                    return default;
                }
            }
            catch { return default; }
        }

        public abstract T[] Strategy(ushort startAdress, ushort NumberOfPoints);
    }

    public abstract class OutputSignalX<T> : Signal<T>, IOutputSignal<T>
    {
        private static readonly Logger _logger = LogManager.GetLogger("InputSignal");

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    SetOutput(PinNumber, _value);
                    OnSignalChanged(_value);
                    _logger.Info($"{Name}; пин {PinNumber} изменил значение с {oldValue} на {_value}");
                }
            }
        }
        public OutputSignalX(string name, ushort pinNumber, NetConfig netConfig) : base(name, pinNumber, netConfig) { }

        public void SetOutput(ushort pinNumber, T value)
        {
            if (_master != null)
            {
                Strategy(pinNumber, value);
            }
        }

        public abstract void Strategy(ushort startAdress, T value);
    }
}
