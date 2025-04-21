using Modbus.Device;
using NLog;
using System;

namespace Oratoria36.Models.Signals
{
    public class OutputSignal<T> : IOutputStrategy<T>
    {
        Logger _logger = LogManager.GetLogger("InputSignal");
        T _value;
        ModbusIpMaster _master;
        public T Value
        {
            get => _value;
            set
            {
                if (!object.Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    WriteValue();
                    OnSignalChanged?.Invoke(_value);
                    _logger.Info($"{Name}; Пин {PinNumber} изменил значение с {oldValue} на {_value}");
                }
            }
        }
        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }
        public OutputSignal(string name, ushort pinNumber, ModbusIpMaster master)
        {
            Name = name;
            PinNumber = pinNumber;
            _master = master;
        }
        private void WriteValue()
        {
            SetOutput(PinNumber, Value);
            //SetTestOutput(PinNumber, Value);
        }

        public void SetOutput(ushort pinNumber, T value)
        {
            if (_master != null)
            {
                if (typeof(T) == typeof(bool))
                    _master.WriteSingleCoil(pinNumber, (bool)(object)value);
                else if (typeof(T) == typeof(ushort))
                    _master.WriteSingleRegister(pinNumber, (ushort)(object)value);
            }
        }
        public void SetTestOutput(ushort pinNumber, T value)
        {
            Value = value;
        }
    }
}
