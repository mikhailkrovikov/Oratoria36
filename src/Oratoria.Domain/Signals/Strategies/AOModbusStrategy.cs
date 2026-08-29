using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class AOModbusStrategy : IOutputStrategy<double>
    {
        private ModbusTCPConfig _netConfig;
        public AOModbusStrategy(ModbusTCPConfig netConfig)
        {
            _netConfig = netConfig;
        }
        public void SetOutput(ushort pinNumber, double value)
        {
            try
            {
                var outValue = (ushort)(value * 4095 / 10);
                _netConfig.Master.WriteSingleRegister((ushort)(0x0800 + 2 + pinNumber), outValue);
            }
            catch { }  
        }
    }
}
