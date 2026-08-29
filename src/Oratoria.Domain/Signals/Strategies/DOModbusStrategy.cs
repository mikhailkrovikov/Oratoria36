using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class DOModbusStrategy : IOutputStrategy<bool>
    {
        private ModbusTCPConfig _netConfig;
        
        public DOModbusStrategy(ModbusTCPConfig netConfig)
        {
            _netConfig = netConfig;
        }
        public void SetOutput(ushort pinNumber, bool value)
        {
            try
            {
                _netConfig.Master.WriteSingleCoil((ushort)(0x1000 + pinNumber), value);
            }
            catch { }
        }
    }
}
