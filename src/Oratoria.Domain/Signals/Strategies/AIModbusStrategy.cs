using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies;

public class AIModbusStrategy : IInputStrategy<double>
{
    private ModbusTCPConfig _netConfig;
    public AIModbusStrategy(ModbusTCPConfig netConfig)
    {
        _netConfig = netConfig;
    }
    public double GetInput(ushort pinNumber)
    {
        var result = _netConfig?.Master?.ReadHoldingRegisters((ushort)(pinNumber + 2), 1);
        return (double)result[0] * 10 / 4096;
    }
}
