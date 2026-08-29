using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module1.Signals
{
    public class Module1AO : IEnumerable<OutputSignal<double>>
    {
        IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;

        public OutputSignal<double> Upravlenie_Bpn { get; set; }
        public OutputSignal<double> Upravlenie_Uun { get; set; }
        public OutputSignal<double> Upravlenie_Bpuog { get; set; }
        public OutputSignal<double> Uroven_Vch_Evm { get; set; }
        public OutputSignal<double> Sbros_Komp { get; set; }

        public Module1AO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Upravlenie_Bpn = new OutputSignal<double>("Управление БПН", 0, _strategy);
            Upravlenie_Uun = new OutputSignal<double>("Управление УУН-1", 1, _strategy);
            Upravlenie_Bpuog = new OutputSignal<double>("Управление БПУОГ", 2, _strategy);
            Uroven_Vch_Evm = new OutputSignal<double>("Уровень ВЧГ ЭВМ", 3, _strategy);
            Sbros_Komp = new OutputSignal<double>("Сброс комп???", 4, _strategy);

            AnalogOutputs = 
            [
                Upravlenie_Bpn,
                Upravlenie_Uun,
                Upravlenie_Bpuog,
                Uroven_Vch_Evm,
                Sbros_Komp,
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
