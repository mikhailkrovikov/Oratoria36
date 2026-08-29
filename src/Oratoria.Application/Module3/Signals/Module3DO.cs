using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module3.Signals
{
    public class Module3DO : IEnumerable<OutputSignal<bool>>
    {
        IOutputStrategy<bool> _strategy;

        public ObservableCollection<OutputSignal<bool>> DigitalOutputs;
        public OutputSignal<bool> Vraschenie_magnetronov { get; set; }
        public OutputSignal<bool> Avariya_vakuumetra { get; set; }
        public OutputSignal<bool> Soglasovanie_bolshe { get; set; }
        public OutputSignal<bool> Kontrol_zagazhivaniya_vakuuma { get; set; }
        public OutputSignal<bool> Obegazhivanie_vakuuma { get; set; }
        public OutputSignal<bool> Termopara_vklyuchit { get; set; }
        public OutputSignal<bool> Anod_vklyuchit { get; set; }
        public OutputSignal<bool> Upravlenie_EVM_vklyuchit { get; set; }
        public OutputSignal<bool> Uroven_EVM_vklyuchit { get; set; }
        public OutputSignal<bool> VCH_vyklyuchit { get; set; }
        public OutputSignal<bool> BPN_vklyuchit { get; set; }
        public OutputSignal<bool> BPM1_vklyuchit { get; set; }
        public OutputSignal<bool> BPM2_vklyuchit { get; set; }
        public OutputSignal<bool> BPM3_vklyuchit { get; set; }
        public OutputSignal<bool> Natekatel_1_vklyuchit { get; set; }
        public OutputSignal<bool> Natekatel_2_vklyuchit { get; set; }
        public OutputSignal<bool> BP_UOG_vklyuchit { get; set; }
        public OutputSignal<bool> UURG_vklyuchit { get; set; }
        public OutputSignal<bool> Privod_3_vklyuchit { get; set; }
        public OutputSignal<bool> FK_KN_otkryt { get; set; }
        public OutputSignal<bool> Zaslonka_otkryt { get; set; }
        public OutputSignal<bool> ShZ_otkryt { get; set; }
        public OutputSignal<bool> Podduv_vklyuchit { get; set; }
        public OutputSignal<bool> Privod_1_vklyuchit { get; set; }
        public OutputSignal<bool> Privod_2_vklyuchit { get; set; }
        public OutputSignal<bool> Privod_4_vklyuchit { get; set; }
        public OutputSignal<bool> Poziciya_1 { get; set; }
        public OutputSignal<bool> Poziciya_2 { get; set; }
        public OutputSignal<bool> Poziciya_3 { get; set; }
        public OutputSignal<bool> Revers_vklyuchit { get; set; }
        public OutputSignal<bool> Tormoz_vklyuchit { get; set; }
        public OutputSignal<bool> Kriogennyj_nasos_vklyuchit { get; set; }

        public Module3DO(ModbusTCPConfig netConfig, IOutputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Vraschenie_magnetronov = new OutputSignal<bool>("Вращение магнетронов", 0, _strategy);
            Avariya_vakuumetra = new OutputSignal<bool>("Авария вакууметра", 1, _strategy);
            Soglasovanie_bolshe = new OutputSignal<bool>("Согласование больше", 2, _strategy);
            Kontrol_zagazhivaniya_vakuuma = new OutputSignal<bool>("Контроль загаживания вакуума", 3, _strategy);
            Obegazhivanie_vakuuma = new OutputSignal<bool>("Обезгаживание вакуума", 4, _strategy);
            Termopara_vklyuchit = new OutputSignal<bool>("Термопара включить", 5, _strategy);
            Anod_vklyuchit = new OutputSignal<bool>("Анод включить", 6, _strategy);
            Upravlenie_EVM_vklyuchit = new OutputSignal<bool>("Управление ЭВМ включить", 7, _strategy);
            Uroven_EVM_vklyuchit = new OutputSignal<bool>("Уровень ЭВМ включить", 8, _strategy);
            VCH_vyklyuchit = new OutputSignal<bool>("ВЧ выключить", 9, _strategy);
            BPN_vklyuchit = new OutputSignal<bool>("БПН включить", 10, _strategy);
            BPM1_vklyuchit = new OutputSignal<bool>("БПМ1 включить", 11, _strategy);
            BPM2_vklyuchit = new OutputSignal<bool>("БПМ2 включить", 12, _strategy);
            BPM3_vklyuchit = new OutputSignal<bool>("БПМ3 включить", 13, _strategy);
            Natekatel_1_vklyuchit = new OutputSignal<bool>("Натекатель 1 включить", 14, _strategy);
            Natekatel_2_vklyuchit = new OutputSignal<bool>("Натекатель 2 включить", 15, _strategy);
            BP_UOG_vklyuchit = new OutputSignal<bool>("БП УОГ включить", 16, _strategy);
            UURG_vklyuchit = new OutputSignal<bool>("УУРГ включить", 17, _strategy);
            Privod_3_vklyuchit = new OutputSignal<bool>("Привод 3 включить", 18, _strategy);
            FK_KN_otkryt = new OutputSignal<bool>("ФК КН открыть", 19, _strategy);
            Zaslonka_otkryt = new OutputSignal<bool>("Заслонка открыть", 20, _strategy);
            ShZ_otkryt = new OutputSignal<bool>("ЩЗ открыть", 21, _strategy);
            Podduv_vklyuchit = new OutputSignal<bool>("Поддув включить (затвор крионасоса)", 22, _strategy);
            Privod_1_vklyuchit = new OutputSignal<bool>("Привод 1 включить", 23, _strategy);
            Privod_2_vklyuchit = new OutputSignal<bool>("Привод 2 включить", 24, _strategy);
            Privod_4_vklyuchit = new OutputSignal<bool>("Привод 4 включить", 25, _strategy);
            Poziciya_1 = new OutputSignal<bool>("Позиция 1", 26, _strategy);
            Poziciya_2 = new OutputSignal<bool>("Позиция 2", 27, _strategy);
            Poziciya_3 = new OutputSignal<bool>("Позиция 3", 28, _strategy);
            Revers_vklyuchit = new OutputSignal<bool>("Реверс включить", 29, _strategy);
            Tormoz_vklyuchit = new OutputSignal<bool>("Тормоз включить", 30, _strategy);
            Kriogennyj_nasos_vklyuchit = new OutputSignal<bool>("Криогенный насос включить", 31, _strategy);

            DigitalOutputs =
            [
                Vraschenie_magnetronov,
                Avariya_vakuumetra,
                Soglasovanie_bolshe,
                Kontrol_zagazhivaniya_vakuuma,
                Obegazhivanie_vakuuma,
                Termopara_vklyuchit,
                Anod_vklyuchit,
                Upravlenie_EVM_vklyuchit,
                Uroven_EVM_vklyuchit,
                VCH_vyklyuchit,
                BPN_vklyuchit,
                BPM1_vklyuchit,
                BPM2_vklyuchit,
                BPM3_vklyuchit,
                Natekatel_1_vklyuchit,
                Natekatel_2_vklyuchit,
                BP_UOG_vklyuchit,
                UURG_vklyuchit,
                Privod_3_vklyuchit,
                FK_KN_otkryt,
                Zaslonka_otkryt,
                ShZ_otkryt,
                Podduv_vklyuchit,
                Privod_1_vklyuchit,
                Privod_2_vklyuchit,
                Privod_4_vklyuchit,
                Poziciya_1,
                Poziciya_2,
                Poziciya_3,
                Revers_vklyuchit,
                Tormoz_vklyuchit,
                Kriogennyj_nasos_vklyuchit
            ];
        }

        public IEnumerator<OutputSignal<bool>> GetEnumerator()
        {
            return DigitalOutputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
