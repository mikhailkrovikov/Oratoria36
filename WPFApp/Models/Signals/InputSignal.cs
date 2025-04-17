using Modbus.Device;
using NLog;
using System;
using System.Diagnostics.Metrics;
using System.Threading.Channels;

namespace Oratoria36.Models.Signals
{
    public class InputSignal<T> : IInputStrategy<T> /*IModbusStrategy*/
    {
        Logger _logger = LogManager.GetLogger("InputSignal");
        private T _value;
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
        public InputSignal(string name, ushort pinNumber, ModbusIpMaster master)
        {
            Name = name;
            PinNumber = pinNumber;
            try
            {
                _master = master;
            }
            catch
            {
                _logger.Warn("Master не инициализирован");
            }
        }
        private void UpdateValue()
        {
            Value = GetInput(PinNumber);           
        }

        public T GetInput(ushort pinNumber)
        {
            if (_master != null)
            {
                if (typeof(T) == typeof(bool))              
                    return(T)(object)_master.ReadInputs(pinNumber, 1)[0];
                else if (typeof(T) == typeof(ushort))
                    return (T)(object)_master.ReadHoldingRegisters(pinNumber, 1)[0];
                else return default;
            }
            else return default;
        }
    }
}
