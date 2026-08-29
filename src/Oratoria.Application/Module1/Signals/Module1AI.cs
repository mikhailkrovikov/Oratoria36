using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module1.Signals
{
    public class Module1AI : IEnumerable<InputSignal<double>>
    {
        private readonly IInputStrategy<double> _strategy;

        public ObservableCollection<InputSignal<double>> AnalogInputs;

        public InputSignal<double> Tok_Bpn { get; set; }
        public InputSignal<double> Napr_Bpn { get; set; }
        public InputSignal<double> Temperatura { get; set; }
        public InputSignal<double> Tok_Bpuog { get; set; }
        public InputSignal<double> Napr_Bpuog { get; set; }
        public InputSignal<double> Pryamaya_Evm { get; set; }
        public InputSignal<double> Otrazhenie_Evm { get; set; }
        public InputSignal<double> Uroven_Smeshcheniya { get; set; }
        public InputSignal<double> Vitsb_Analog { get; set; }
        public InputSignal<double> Vitsb_Analog_Dekodirovanie { get; set; }
        public InputSignal<double> Faza_Evm { get; set; }
        public InputSignal<double> Modul_Evm { get; set; }
        public InputSignal<double> Uroven_Nastroyki { get; set; }
        public InputSignal<double> Uroven_Soglasovaniya { get; set; }

        public Module1AI(ModbusTCPConfig netConfig, IInputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Tok_Bpn = new InputSignal<double>("Ток БПН", 0, _strategy);
            Napr_Bpn = new InputSignal<double>("Напряжение БПН", 1, _strategy);
            Temperatura = new InputSignal<double>("Температура", 2, _strategy);
            Tok_Bpuog = new InputSignal<double>("Ток БПУОГ", 3, _strategy);
            Napr_Bpuog = new InputSignal<double>("Напряжение БПУОГ", 4, _strategy);
            Pryamaya_Evm = new InputSignal<double>("Прямая ЭВМ", 5, _strategy);
            Otrazhenie_Evm = new InputSignal<double>("Отраженная ЭВМ", 6, _strategy);
            Uroven_Smeshcheniya = new InputSignal<double>("Уровень смещения", 7, _strategy);
            Vitsb_Analog = new InputSignal<double>("ВИЦБ", 8, _strategy);
            Vitsb_Analog_Dekodirovanie = new InputSignal<double>("ВИЦБ анал. дек.???", 9, _strategy);
            Faza_Evm = new InputSignal<double>("Фаза ЭВМ", 10, _strategy);
            Modul_Evm = new InputSignal<double>("Модуль ЭВМ", 11, _strategy);
            Uroven_Nastroyki = new InputSignal<double>("Уровень настройки", 12, _strategy);
            Uroven_Soglasovaniya = new InputSignal<double>("Уровень согласования", 13, _strategy);

            AnalogInputs =
            [
                Tok_Bpn,
                Napr_Bpn,
                Temperatura,
                Tok_Bpuog,
                Napr_Bpuog,
                Pryamaya_Evm,
                Otrazhenie_Evm,
                Uroven_Smeshcheniya,
                Vitsb_Analog,
                Vitsb_Analog_Dekodirovanie,
                Faza_Evm,
                Modul_Evm,
                Uroven_Nastroyki,
                Uroven_Soglasovaniya,
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
