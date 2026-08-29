using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class DIModbusStrategy : IInputStrategy<bool>
    {
        private ModbusTCPConfig _netConfig;
        public DIModbusStrategy(ModbusTCPConfig netConfig)
        {
            _netConfig = netConfig;
        }
        public bool GetInput(ushort pinNumber)
        {
            var result = _netConfig.Master.ReadInputs(pinNumber, 1);
            return result[0];
        }
    }
}
