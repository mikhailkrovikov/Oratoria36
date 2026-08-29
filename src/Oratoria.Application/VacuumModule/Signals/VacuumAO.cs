using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.VacuumModule.Signals
{
    public class VacuumAO : IEnumerable<OutputSignal<double>>
    {
        IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;

        public VacuumAO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif
            AnalogOutputs = [];
        }

        public IEnumerator<OutputSignal<double>> GetEnumerator()
        {
            return AnalogOutputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
