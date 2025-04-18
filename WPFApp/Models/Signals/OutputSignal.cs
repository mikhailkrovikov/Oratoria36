using Modbus.Device;
using NLog;
using System;

namespace Oratoria36.Models.Signals
{
    public class OutputSignal<T> : IOutputStrategy<T> /*IModbusStrategy*/
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
            SetOutput(PinNumber, Value);
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
    }
}
