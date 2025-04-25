using Modbus.Device;
using NLog;
using System;

namespace Oratoria36.Models.Signals
{
    public class InputSignal<T> : IInputStrategy<T>
    {
        private static readonly Logger _logger = LogManager.GetLogger("InputSignal");

        private T _value;
        private ModbusIpMaster _master;

        public T Value
        {
            get
            {
                if (_master == null)
                    return _value;               
                else
                    return GetInput(PinNumber);         
            }
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    OnSignalChanged?.Invoke(_value);
                    _logger.Info($"{Name}; Пин {PinNumber} изменил значение с {oldValue} на {_value}");
                }
            }
        }

        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }

        public InputSignal(string name, ushort pinNumber, ModbusIpMaster master = null)
        {
            Name = name;
            PinNumber = pinNumber;
            _master = master;
        }

        public T GetInput(ushort pinNumber)
        {
            if (_master != null)
            {
                if (typeof(T) == typeof(bool))
                {
                    var result = _master.ReadInputs(pinNumber, 1);
                    return (T)(object)result[0];
                }
                else if (typeof(T) == typeof(ushort))
                {
                    var result = _master.ReadHoldingRegisters(pinNumber, 1);
                    return (T)(object)result[0];
                }
                else return default;
            }
            else
            {
                return default;
            }
        }
    }
}
