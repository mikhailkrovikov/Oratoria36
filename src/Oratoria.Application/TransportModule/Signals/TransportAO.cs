using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.TransportModule.Signals
{
    public class TransportAO : IEnumerable<OutputSignal<double>>
    {
        private readonly IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;

        public OutputSignal<double> UURG1_Setpoint { get; set; }
        public OutputSignal<double> UURG2_Setpoint { get; set; }

        public TransportAO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            UURG1_Setpoint = new OutputSignal<double>("УУРГ-1: задание", 0, _strategy);
            UURG2_Setpoint = new OutputSignal<double>("УУРГ-2: задание", 1, _strategy);

            AnalogOutputs =
            [
                UURG1_Setpoint,
                UURG2_Setpoint,
            ];
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
