using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module1.Signals
{
    public class Module1DI : IEnumerable<InputSignal<bool>>
    {
        private IInputStrategy<bool> _strategy;

        public ObservableCollection<InputSignal<bool>> DigitalInputs;
        public InputSignal<bool> BPN_Vkluchen {  get; set; }
        public InputSignal<bool> UUN1_Vkluchen {  get; set; }
        public InputSignal<bool> BP_UOG_Vkluchen {  get; set; }
        public InputSignal<bool> FK_KN_DU_63_Otkryt {  get; set; }
        public InputSignal<bool> FK_KN_DU_63_Zakryt {  get; set; }
        public InputSignal<bool> Zatvor_Otkryt { get; set; }
        public InputSignal<bool> Zatvor_Zakryt { get; set; }
        public InputSignal<bool> Znak_Smescheniya { get; set; }
        public InputSignal<bool> Nakal_Est { get; set; }
        public InputSignal<bool> Upravlenie_EVM { get; set; }
        public InputSignal<bool> Uroven_EVM { get; set; }
        public InputSignal<bool> Sogl_Vykl { get; set; }
        public InputSignal<bool> Anod_Est { get; set; }
        public InputSignal<bool> VCH_Vkl { get; set; }
        public InputSignal<bool> VCH_Vykl { get; set; }
        public InputSignal<bool> KN_Vkl { get; set; }
        public InputSignal<bool> Voda_Est { get; set; }
        public InputSignal<bool> Position1 { get; set; }
        public InputSignal<bool> Position2 { get; set; }
        public InputSignal<bool> Position3 { get; set; }
        public InputSignal<bool> Revers_Vkl { get; set; }
        public InputSignal<bool> Tormos_Vkl { get; set; }
        public InputSignal<bool> Peregruz_Privoda { get; set; }

        

        public Module1DI(ModbusTCPConfig netConfig, IInputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            BPN_Vkluchen = new InputSignal<bool>("БПН включен", 0, _strategy);
            UUN1_Vkluchen = new InputSignal<bool>("УУН-1 включен", 1, _strategy);
            BP_UOG_Vkluchen = new InputSignal<bool>(" БП УОГ включен", 2, _strategy);
            FK_KN_DU_63_Otkryt = new InputSignal<bool>(" ФК КН открыт", 3, _strategy);
            FK_KN_DU_63_Zakryt = new InputSignal<bool>(" ФК КН закрыт", 4, _strategy);
            Zatvor_Otkryt = new InputSignal<bool>("ЩЗ открыт", 5, _strategy);
            Zatvor_Zakryt = new InputSignal<bool>("ЩЗ закрыт", 6, _strategy);
            Znak_Smescheniya = new InputSignal<bool>("Знак смещения", 7, _strategy);
            Nakal_Est = new InputSignal<bool>(" Накал есть", 8, _strategy);
            Upravlenie_EVM = new InputSignal<bool>("Управление ЭВМ", 9, _strategy);
            Uroven_EVM = new InputSignal<bool>("Уровень ЭВМ", 10, _strategy);
            Sogl_Vykl = new InputSignal<bool>("Согласнование выключено", 11, _strategy);
            Anod_Est = new InputSignal<bool>("Анод есть", 12, _strategy);
            VCH_Vkl = new InputSignal<bool>("ВЧГ включен", 13, _strategy);
            VCH_Vykl = new InputSignal<bool>("ВЧГ выключен", 14, _strategy);
            KN_Vkl = new InputSignal<bool>("Криогенный насос включен", 15, _strategy);
            Voda_Est = new InputSignal<bool>("Вода есть", 16, _strategy);
            Position1 = new InputSignal<bool>("Позиция 1", 17, _strategy);
            Position2 = new InputSignal<bool>("Позиция 2", 18, _strategy);
            Position3 = new InputSignal<bool>("Позиция 3", 19, _strategy);
            Revers_Vkl = new InputSignal<bool>("Реверс включен", 20, _strategy);
            Tormos_Vkl = new InputSignal<bool>("Тормоз включен", 21, _strategy);
            Peregruz_Privoda = new InputSignal<bool>("Перегруз привода", 22, _strategy);

            DigitalInputs = 
            [
                BPN_Vkluchen,
                UUN1_Vkluchen,
                BP_UOG_Vkluchen,
                FK_KN_DU_63_Otkryt,
                FK_KN_DU_63_Zakryt,
                Zatvor_Otkryt,
                Zatvor_Zakryt,
                Znak_Smescheniya,
                Nakal_Est,
                Upravlenie_EVM,
                Uroven_EVM,
                Sogl_Vykl,
                Anod_Est,
                VCH_Vkl,
                VCH_Vykl,
                KN_Vkl,
                Voda_Est,
                Position1,
                Position2,
                Position3,
                Revers_Vkl,
                Tormos_Vkl,
                Peregruz_Privoda,
            ];

        }

        public IEnumerator<InputSignal<bool>> GetEnumerator()
        {
            return DigitalInputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
