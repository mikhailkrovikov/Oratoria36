using Modbus.Device;
using Oratoria36.Models.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2DO
    {
        public List<OutputSignal<bool>> DigitalOutputs;
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

        public Module2DO(ModbusIpMaster master)
        {
            Vraschenie_magnetronov = new OutputSignal<bool>("Вращение магнетронов", 0, master);
            Avariya_vakuumetra = new OutputSignal<bool>("Авария вакууметра", 1, master);
            Soglasovanie_bolshe = new OutputSignal<bool>("Согласование больше", 2, master);
            Kontrol_zagazhivaniya_vakuuma = new OutputSignal<bool>("Контроль загаживания вакуума", 3, master);
            Obegazhivanie_vakuuma = new OutputSignal<bool>("Обезгаживание вакуума", 4, master);
            Termopara_vklyuchit = new OutputSignal<bool>("Термопара включить", 5, master);
            Anod_vklyuchit = new OutputSignal<bool>("Анод включить", 6, master);
            Upravlenie_EVM_vklyuchit = new OutputSignal<bool>("Управление ЭВМ включить", 7, master);
            Uroven_EVM_vklyuchit = new OutputSignal<bool>("Уровень ЭВМ включить", 8, master);
            VCH_vyklyuchit = new OutputSignal<bool>("ВЧ выключить", 9, master);
            BPN_vklyuchit = new OutputSignal<bool>("БПН включить", 10, master);
            BPM1_vklyuchit = new OutputSignal<bool>("БПМ1 включить", 11, master);
            BPM2_vklyuchit = new OutputSignal<bool>("БПМ2 включить", 12, master);
            BPM3_vklyuchit = new OutputSignal<bool>("БПМ3 включить", 13, master);
            Natekatel_1_vklyuchit = new OutputSignal<bool>("Натекатель 1 включить", 14, master);
            Natekatel_2_vklyuchit = new OutputSignal<bool>("Натекатель 2 включить", 15, master);
            BP_UOG_vklyuchit = new OutputSignal<bool>("БП УОГ включить", 16, master);
            UURG_vklyuchit = new OutputSignal<bool>("УУРГ включить", 17, master);
            Privod_3_vklyuchit = new OutputSignal<bool>("Привод 3 включить", 18, master);
            FK_KN_otkryt = new OutputSignal<bool>("ФК КН открыть", 19, master);
            Zaslonka_otkryt = new OutputSignal<bool>("Заслонка открыть", 20, master);
            ShZ_otkryt = new OutputSignal<bool>("ЩЗ открыть", 21, master);
            Podduv_vklyuchit = new OutputSignal<bool>("Поддув включить (затвор крионасоса)", 22, master);
            Privod_1_vklyuchit = new OutputSignal<bool>("Привод 1 включить", 23, master);
            Privod_2_vklyuchit = new OutputSignal<bool>("Привод 2 включить", 24, master);
            Privod_4_vklyuchit = new OutputSignal<bool>("Привод 4 включить", 25, master);
            Poziciya_1 = new OutputSignal<bool>("Позиция 1", 26, master);
            Poziciya_2 = new OutputSignal<bool>("Позиция 2", 27, master);
            Poziciya_3 = new OutputSignal<bool>("Позиция 3", 28, master);
            Revers_vklyuchit = new OutputSignal<bool>("Реверс включить", 29, master);
            Tormoz_vklyuchit = new OutputSignal<bool>("Тормоз включить", 30, master);
            Kriogennyj_nasos_vklyuchit = new OutputSignal<bool>("Криогенный насос включить", 31, master);

            DigitalOutputs = new List<OutputSignal<bool>>()
            {
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
            };
        }
    }
}
