using Modbus.Device;
using NLog;
using System;

namespace Oratoria36.Models.Signals
{
    public class InputSignal<T> : IModbusStrategy
    {
        Logger _logger = LogManager.GetLogger("InputSignal");
        private T _value;
        private IModbusStrategy _modbusPoller;
        private ModbusIpMaster _master;
        public T Value
        {
            get
            {
                UpdateValue();
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnSignalChanged?.Invoke(_value);
                }
            }
        }
        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }
        public InputSignal(string name, ushort pinNumber, IModbusStrategy modbusPoller, ModbusIpMaster master)
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
        private void UpdateValue()
        {
            try
            {
                if (typeof(T) == typeof(bool))
                {
                    Value = (T)(object)_modbusPoller.GetDigitalInput(PinNumber, _master);
                }
                else if (typeof(T) == typeof(ushort))
                {
                    Value = (T)(object)_modbusPoller.GetAnalogInput(PinNumber, _master);
                }
            }
            catch { }
        }
    }
}
