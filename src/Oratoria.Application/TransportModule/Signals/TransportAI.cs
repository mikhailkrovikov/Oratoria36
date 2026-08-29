using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;
namespace Oratoria.Application.TransportModule.Signals
{
    public class TransportAI : IEnumerable<InputSignal<double>>
    {
        private readonly IInputStrategy<double> _strategy;

        public ObservableCollection<InputSignal<double>> AnalogInputs;

        public InputSignal<double> UURG1_RealValue { get; set; }
        public InputSignal<double> UURG2_RealValue { get; set; }

        public TransportAI(ModbusTCPConfig netConfig, IInputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            UURG1_RealValue = new InputSignal<double>("УУРГ-1: текущее", 0, _strategy);
            UURG2_RealValue = new InputSignal<double>("УУРГ-2: текущее", 1, _strategy);

            AnalogInputs = 
            [
                UURG1_RealValue,
                UURG2_RealValue,
            ];
        }

        public IEnumerator<InputSignal<double>> GetEnumerator()
        {
            return AnalogInputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
