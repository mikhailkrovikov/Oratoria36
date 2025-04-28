using Modbus.Device;
using NLog;
using Oratoria36.Models.Connection;
using System;

namespace Oratoria36.Models.Signals
{
    public class OutputSignal<T> : IOutputStrategy<T>
    {
        Logger _logger = LogManager.GetLogger("OutputSignal");
        T _value;
        private NetConfig _netConfig;
        private ModbusIpMaster _master => _netConfig.Master;
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
                    OnSignalChanged?.Invoke(_value);
                    _logger.Info($"{Name}; пин {PinNumber} изменил значение с {oldValue} на {_value}");
                }
            }
        }
        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }
        public OutputSignal(string name, ushort pinNumber, NetConfig netConfig)
        {
            Name = name;
            PinNumber = pinNumber;
            _netConfig = netConfig;
        }
        public void SetOutput(ushort pinNumber, T value)
        {
            if (_master != null)
            {
                if (typeof(T) == typeof(bool))
                    _master.WriteSingleCoil((ushort)(0x1000 + pinNumber), (bool)(object)value);
                else if (typeof(T) == typeof(ushort))
                    _master.WriteSingleRegister((ushort)(0x0800 + 2 + pinNumber), (ushort)(object)value);
            }
        }
    }
}
