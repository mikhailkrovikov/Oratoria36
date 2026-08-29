using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module4.Signals
{
    public class Module4AO : IEnumerable<OutputSignal<double>>
    {
        IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;
        public OutputSignal<double> Moshchnost_BPN { get; set; }
        public OutputSignal<double> Moshchnost_BPM1 { get; set; }
        public OutputSignal<double> Moshchnost_BPM2 { get; set; }
        public OutputSignal<double> Moshchnost_BPM3 { get; set; }
        public OutputSignal<double> Upravlenie_natekatelem { get; set; }
        public OutputSignal<double> Raskhod_gasa_ustavka { get; set; }

        public Module4AO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif
            Moshchnost_BPN = new OutputSignal<double>("Мощность БПН", 0, _strategy);
            Moshchnost_BPM1 = new OutputSignal<double>("Мощность БПМ1", 1, _strategy);
            Moshchnost_BPM2 = new OutputSignal<double>("Мощность БПМ2", 2, _strategy);
            Moshchnost_BPM3 = new OutputSignal<double>("Мощность БПМ3", 3, _strategy);
            Upravlenie_natekatelem = new OutputSignal<double>("Управление натекателем", 4, _strategy);
            Raskhod_gasa_ustavka = new OutputSignal<double>("Расход газа: уставка", 5, _strategy);

            AnalogOutputs =
            [
                Moshchnost_BPN,
                Moshchnost_BPM1,
                Moshchnost_BPM2,
                Moshchnost_BPM3,
                Upravlenie_natekatelem,
                Raskhod_gasa_ustavka
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
