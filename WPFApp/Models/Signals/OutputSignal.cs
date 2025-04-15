using Modbus.Device;
using NLog;
using System;

namespace Oratoria36.Models.Signals
{
    public class OutputSignal<T> : IModbusStrategy
    {
        Logger _logger = LogManager.GetLogger("InputSignal");
        private T _value;
        private IModbusStrategy _modbusPoller;
        private ModbusIpMaster _master;
        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    WriteValue();
                    OnSignalChanged?.Invoke(_value);
                }
            }
        }
        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }
        public OutputSignal(string name, ushort pinNumber, IModbusStrategy modbusPoller, ModbusIpMaster master)
        {
            Name = name;
            PinNumber = pinNumber;
            _modbusPoller = modbusPoller;
            try
            {
                _master = master;
            }
            catch
            {
                _logger.Error("Master не инициализирован");
            }
        }
        private void WriteValue()
        {
            if (typeof(T) == typeof(bool))
                _modbusPoller.SetDigitalOutput(PinNumber, (bool)(object)_value, _master);
            else if (typeof(T) == typeof(ushort))
                _modbusPoller.SetAnalogOutput(PinNumber, (ushort)(object)_value, _master);
        }
    }
}
