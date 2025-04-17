using Modbus.Device;
using Oratoria36.Models.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2DI /*IModbusStrategy*/
    {

        public List<InputSignal<bool>> DigitalInputs;
        public InputSignal<bool> Nakal_est { get; set; }
        public InputSignal<bool> Upravlenie_EVM { get; set; }
        public InputSignal<bool> Uroven_EVM { get; set; }
        public InputSignal<bool> Dvizhenie_BPM { get; set; }
        public InputSignal<bool> Anod_vklyuchen { get; set; }
        public InputSignal<bool> VCH_vklyuchen { get; set; }
        public InputSignal<bool> VCH_vyklyuchen { get; set; }
        public InputSignal<bool> UURG_vklyucheno { get; set; }
        public InputSignal<bool> BPN_vklyuchen { get; set; }
        public InputSignal<bool> BPM1_vklyuchen { get; set; }
        public InputSignal<bool> Peregrev_BPM_est { get; set; }
        public InputSignal<bool> Peregruzka_BPM_est { get; set; }
        public InputSignal<bool> BPM2_vklyuchen { get; set; }
        public InputSignal<bool> Poziciya_1 { get; set; }
        public InputSignal<bool> Poziciya_2 { get; set; }
        public InputSignal<bool> Poziciya_3 { get; set; }
        public InputSignal<bool> BPM3_vklyuchen { get; set; }
        public InputSignal<bool> Revers_vklyuchen { get; set; }
        public InputSignal<bool> Kriogennyj_nasos_vklyuchen { get; set; }
        public InputSignal<bool> Natekatel_1_vklyuchen { get; set; }
        public InputSignal<bool> Natekatel_2_vklyuchen { get; set; }
        public InputSignal<bool> BP_UOG_vklyuchen { get; set; }
        public InputSignal<bool> FK_KN_DU_63_otkryt { get; set; }
        public InputSignal<bool> FK_KN_DU_63_zakryt { get; set; }
        public InputSignal<bool> Zaslonka_otkryta { get; set; }
        public InputSignal<bool> Zaslonka_zakryta { get; set; }
        public InputSignal<bool> SHCHZ_otkryt { get; set; }
        public InputSignal<bool> SHCHZ_zakryt { get; set; }
        public InputSignal<bool> Peregrev_vody_est { get; set; }
        public InputSignal<bool> Voda_est { get; set; }
        public InputSignal<bool> Tormoz_vklyuchen { get; set; }
        public InputSignal<bool> Peregruz_privoda_est { get; set; }


        public Module2DI(ModbusIpMaster master)
        {
            Nakal_est = new InputSignal<bool>("Накал есть", 0, master);
            Upravlenie_EVM = new InputSignal<bool>("Управление ЭВМ", 1, master);
            Uroven_EVM = new InputSignal<bool>("Уровень ЭВМ", 2, master);
            Dvizhenie_BPM = new InputSignal<bool>("Движение БПМ", 3, master);
            Anod_vklyuchen = new InputSignal<bool>("Анод включен", 4, master);
            VCH_vklyuchen = new InputSignal<bool>("ВЧ включен", 5, master);
            VCH_vyklyuchen = new InputSignal<bool>("ВЧ выключен", 6, master);
            UURG_vklyucheno = new InputSignal<bool>("УУРГ включено", 7, master);
            BPN_vklyuchen = new InputSignal<bool>("БПН включен", 8, master);
            BPM1_vklyuchen = new InputSignal<bool>("БПМ1 включен", 9, master);
            Peregrev_BPM_est = new InputSignal<bool>("Перегрев БПМ есть", 10, master);
            Peregruzka_BPM_est = new InputSignal<bool>("Перегрузка БПМ есть", 11, master);
            BPM2_vklyuchen = new InputSignal<bool>("БПМ2 включен", 12, master);
            Poziciya_1 = new InputSignal<bool>("Позиция 1", 13, master);
            Poziciya_2 = new InputSignal<bool>("Позиция 2", 14, master);
            Poziciya_3 = new InputSignal<bool>("Позиция 3", 15, master);
            BPM3_vklyuchen = new InputSignal<bool>("БПМ3 включен", 16, master);
            Revers_vklyuchen = new InputSignal<bool>("Реверс включен", 17, master);
            Kriogennyj_nasos_vklyuchen = new InputSignal<bool>("Криогенный насос включен", 18, master);
            Natekatel_1_vklyuchen = new InputSignal<bool>("Натекатель 1 включен", 19, master);
            Natekatel_2_vklyuchen = new InputSignal<bool>("Натекатель 2 включен", 20, master);
            BP_UOG_vklyuchen = new InputSignal<bool>("БП УОГ включен", 21, master);
            FK_KN_DU_63_otkryt = new InputSignal<bool>("ФК-КН ДУ-63 открыт", 22, master);
            FK_KN_DU_63_zakryt = new InputSignal<bool>("ФК-КН ДУ-63 закрыт ", 23, master);
            Zaslonka_otkryta = new InputSignal<bool>("Заслонка открыта", 24, master);
            Zaslonka_zakryta = new InputSignal<bool>("Заслонка закрыта ", 25, master);
            SHCHZ_otkryt = new InputSignal<bool>("ЩЗ открыт ", 26, master);
            SHCHZ_zakryt = new InputSignal<bool>("ЩЗ закрыт ", 27, master);
            Peregrev_vody_est = new InputSignal<bool>("Перегрев воды есть ", 28, master);
            Voda_est = new InputSignal<bool>("Вода есть ", 29, master);
            Tormoz_vklyuchen = new InputSignal<bool>("Тормоз включен ", 30, master);
            Peregruz_privoda_est = new InputSignal<bool>("Перегруз привода есть ", 31, master);

            DigitalInputs = new List<InputSignal<bool>>
            {
                Nakal_est,
                Upravlenie_EVM,
                Uroven_EVM,
                Dvizhenie_BPM,
                Anod_vklyuchen,
                VCH_vklyuchen,
                VCH_vyklyuchen,
                UURG_vklyucheno,
                BPN_vklyuchen,
                BPM1_vklyuchen,
                Peregrev_BPM_est,
                Peregruzka_BPM_est,
                BPM2_vklyuchen,
                Poziciya_1,
                Poziciya_2,
                Poziciya_3,
                BPM3_vklyuchen,
                Revers_vklyuchen,
                Kriogennyj_nasos_vklyuchen,
                Natekatel_1_vklyuchen,
                Natekatel_2_vklyuchen,
                BP_UOG_vklyuchen,
                FK_KN_DU_63_otkryt,
                FK_KN_DU_63_zakryt,
                Zaslonka_otkryta,
                Zaslonka_zakryta,
                SHCHZ_otkryt,
                SHCHZ_zakryt,
                Peregrev_vody_est,
                Voda_est,
                Tormoz_vklyuchen,
                Peregruz_privoda_est,

            };
        }
    }
}
