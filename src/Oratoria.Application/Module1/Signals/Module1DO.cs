using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module1.Signals
{
    public class Module1DO : IEnumerable<OutputSignal<bool>>
    {
        IOutputStrategy<bool> _strategy;

        public ObservableCollection<OutputSignal<bool>> DigitalOutputs;

        public OutputSignal<bool> Bpn_Vkl { get; set; }
        public OutputSignal<bool> Uun_1_Vkl { get; set; }
        public OutputSignal<bool> Bp_Uog_Vkl { get; set; }
        public OutputSignal<bool> Fk_Kn1_Vkl { get; set; }
        public OutputSignal<bool> ShhZ_Vkl { get; set; }
        public OutputSignal<bool> Vkl_Kp_Kzkn { get; set; }
        public OutputSignal<bool> Vkl_Privoda_1 { get; set; }
        public OutputSignal<bool> Vkl_Privoda_2 { get; set; }
        public OutputSignal<bool> Nastr_Bolshe { get; set; }
        public OutputSignal<bool> Nast_Menshe { get; set; }
        public OutputSignal<bool> Sogl_Bolshe { get; set; }
        public OutputSignal<bool> Sogl_Menshe { get; set; }
        public OutputSignal<bool> Kontrol_Zagazh { get; set; }
        public OutputSignal<bool> Sogl_Evm_Vykl { get; set; }
        public OutputSignal<bool> Obezgazhivanie { get; set; }
        public OutputSignal<bool> Anod_Evm_Vkl { get; set; }
        public OutputSignal<bool> Vkl_Privoda_4 { get; set; }
        public OutputSignal<bool> Pozitsiya_1 { get; set; }
        public OutputSignal<bool> Pozitsiya_2 { get; set; }
        public OutputSignal<bool> Pozitsiya_3 { get; set; }
        public OutputSignal<bool> Revers_Vkl { get; set; }
        public OutputSignal<bool> Tormoz_Vkl { get; set; }
        public OutputSignal<bool> Vkl_Privoda_3 { get; set; }
        public OutputSignal<bool> Sbros_Avariya { get; set; }
        public OutputSignal<bool> Vkl_Upr_Evm { get; set; }
        public OutputSignal<bool> Vkl_Urov_Evm { get; set; }
        public OutputSignal<bool> Vykl_Vch_Evm { get; set; }
        public OutputSignal<bool> Kn_Vkl { get; set; }

        public Module1DO(ModbusTCPConfig netConfig, IOutputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Bpn_Vkl = new OutputSignal<bool>("БПН включить", 0, _strategy);
            Uun_1_Vkl = new OutputSignal<bool>("УУН-1 включить", 1, _strategy);
            Bp_Uog_Vkl = new OutputSignal<bool>("БПУОГ включить", 2, _strategy);
            Fk_Kn1_Vkl = new OutputSignal<bool>("ФК КН открыть", 3, _strategy);
            ShhZ_Vkl = new OutputSignal<bool>("ЩЗ открыть", 4, _strategy);
            Vkl_Kp_Kzkn = new OutputSignal<bool>("Затвор КН???", 5, _strategy);
            Vkl_Privoda_1 = new OutputSignal<bool>("Привод 1 включить", 6, _strategy);
            Vkl_Privoda_2 = new OutputSignal<bool>("Привод 2 включить", 7, _strategy);
            Nastr_Bolshe = new OutputSignal<bool>("Настройка: больше", 8, _strategy);
            Nast_Menshe = new OutputSignal<bool>("Настройка: меньше", 9, _strategy);
            Sogl_Bolshe = new OutputSignal<bool>("Согласование больше", 10, _strategy);
            Sogl_Menshe = new OutputSignal<bool>("Согласование меньше", 11, _strategy);
            Kontrol_Zagazh = new OutputSignal<bool>("Контроль загаживания", 12, _strategy);
            Sogl_Evm_Vykl = new OutputSignal<bool>("Согласование ЭВМ выключить", 13, _strategy);
            Obezgazhivanie = new OutputSignal<bool>("Обезгаживание включить", 14, _strategy);
            Anod_Evm_Vkl = new OutputSignal<bool>("Анод ЭВМ включить", 15, _strategy);
            Vkl_Privoda_4 = new OutputSignal<bool>("Привод 4 включить", 16, _strategy);
            Pozitsiya_1 = new OutputSignal<bool>("Позиция 1", 17, _strategy);
            Pozitsiya_2 = new OutputSignal<bool>("Позиция 2", 18, _strategy);
            Pozitsiya_3 = new OutputSignal<bool>("Позиция 3", 19, _strategy);
            Revers_Vkl = new OutputSignal<bool>("Реверс включить", 20, _strategy);
            Tormoz_Vkl = new OutputSignal<bool>("Тормоз включить", 21, _strategy);
            Vkl_Privoda_3 = new OutputSignal<bool>("Привод 3 включить", 22 , _strategy);
            Sbros_Avariya = new OutputSignal<bool>("Сброс аварии", 23, _strategy);
            Vkl_Upr_Evm = new OutputSignal<bool>("Управление ЭВМ включить", 24, _strategy);
            Vkl_Urov_Evm = new OutputSignal<bool>("Уровень ЭВМ включить", 25, _strategy);
            Vykl_Vch_Evm = new OutputSignal<bool>("ВЧГ ЭВМ выключить", 26, _strategy);
            Kn_Vkl = new OutputSignal<bool>("Криогенный насос включен", 27, _strategy);

            DigitalOutputs =
            [
                Bpn_Vkl,
                Uun_1_Vkl,
                Bp_Uog_Vkl,
                Fk_Kn1_Vkl,
                ShhZ_Vkl,
                Vkl_Kp_Kzkn,
                Vkl_Privoda_1,
                Vkl_Privoda_2,
                Nastr_Bolshe,
                Nast_Menshe,
                Sogl_Bolshe,
                Sogl_Menshe,
                Kontrol_Zagazh,
                Sogl_Evm_Vykl,
                Obezgazhivanie,
                Anod_Evm_Vkl,
                Vkl_Privoda_4,
                Pozitsiya_1,
                Pozitsiya_2,
                Pozitsiya_3,
                Revers_Vkl,
                Tormoz_Vkl,
                Vkl_Privoda_3,
                Sbros_Avariya,
                Vkl_Upr_Evm,
                Vkl_Urov_Evm,
                Vykl_Vch_Evm,
                Kn_Vkl,
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
