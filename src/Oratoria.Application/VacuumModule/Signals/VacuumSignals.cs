using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using Oratoria.Application.Connection;
using Oratoria.Application.Strategies;

namespace Oratoria.Application.VacuumModule.Signals
{
    public class VacuumSignals : IModuleSignals
    {
        private readonly ModbusTCPConfig _netConfig;
        public VacuumDI DISignals { get; }
        public VacuumDO DOSignals { get; }
        public VacuumAI AISignals { get; }
        public VacuumAO AOSignals { get; }

        IEnumerable<InputSignal<bool>> IModuleSignals.DISignals => DISignals;

        IEnumerable<OutputSignal<bool>> IModuleSignals.DOSignals => DOSignals;

        IEnumerable<InputSignal<double>> IModuleSignals.AISignals => AISignals;

        IEnumerable<OutputSignal<double>> IModuleSignals.AOSignals => AOSignals;

#if RELEASE
        public VacuumSignals(NetContext netContext)
        {
            _netConfig = netContext.TransportModule;
            DISignals = new(_netConfig, new DIModbusStrategy(_netConfig));
            DOSignals = new(_netConfig, new DOModbusStrategy(_netConfig));
            AISignals = new(_netConfig, new AIModbusStrategy(_netConfig));
            AOSignals = new(_netConfig, new AOModbusStrategy(_netConfig));
        }
#else
        public VacuumSignals(NetContext netContext, DigitalTwinStrategy twinStrategy)
        {
            _netConfig = netContext.TransportModule;
            DISignals = new(_netConfig, twinStrategy);
            DOSignals = new(_netConfig, twinStrategy);
            AISignals = new(_netConfig, twinStrategy);
            AOSignals = new(_netConfig, twinStrategy);

            twinStrategy.RegisterSignals(DISignals);
            twinStrategy.RegisterSignals(AISignals);
        }
#endif
    }
}
