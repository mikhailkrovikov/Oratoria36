using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module3.Signals
{
    public class Module3AI : IEnumerable<InputSignal<double>>
    {
        IInputStrategy<double> _strategy;

        public ObservableCollection<InputSignal<double>> AnalogInputs;
        public InputSignal<double> Napryazhenie_BPN { get; set; }
        public InputSignal<double> Tok_BPN { get; set; }
        public InputSignal<double> Tok_BPM1 { get; set; }
        public InputSignal<double> Napryazhenie_BPM1 { get; set; }
        public InputSignal<double> Tok_BPM2 { get; set; }
        public InputSignal<double> Napryazhenie_BPM2 { get; set; }
        public InputSignal<double> Tok_BPM3 { get; set; }
        public InputSignal<double> Napryazhenie_BPM3 { get; set; }
        public InputSignal<double> Termopara { get; set; }
        public InputSignal<double> VICB { get; set; }
        public InputSignal<double> Raskhod_gasa_tekushchee { get; set; }
        public Module3AI(ModbusTCPConfig netConfig, IInputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Napryazhenie_BPN = new InputSignal<double>("Напряжение БПН", 0, _strategy);
            Tok_BPN = new InputSignal<double>("Ток БПН", 1, _strategy);
            Tok_BPM1 = new InputSignal<double>("Ток БПМ1", 2, _strategy);
            Napryazhenie_BPM1 = new InputSignal<double>("Напряжение БПМ1", 3, _strategy);
            Tok_BPM2 = new InputSignal<double>("Ток БПМ2", 4, _strategy);
            Napryazhenie_BPM2 = new InputSignal<double>("Напряжение БПМ2", 5, _strategy);
            Tok_BPM3 = new InputSignal<double>("Ток БПМ3", 6, _strategy);
            Napryazhenie_BPM3 = new InputSignal<double>("Напряжение БПМ3", 7, _strategy);
            Termopara = new InputSignal<double>("Термопара", 8, _strategy);
            VICB = new InputSignal<double>("ВИЦБ", 9, _strategy);
            Raskhod_gasa_tekushchee = new InputSignal<double>("Расход газа: текущее", 10, _strategy);

            AnalogInputs =
            [
                Napryazhenie_BPN,
                Tok_BPN,
                Tok_BPM1,
                Napryazhenie_BPM1,
                Tok_BPM2,
                Napryazhenie_BPM2,
                Tok_BPM3,
                Napryazhenie_BPM3,
                Termopara,
                VICB,
                Raskhod_gasa_tekushchee
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
