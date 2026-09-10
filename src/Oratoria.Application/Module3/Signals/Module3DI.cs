using Oratoria.Domain.Devices.Shutter.ShutterAttributes;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Application.Module3.DeviceCollection;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Devices.Abstractions.MechanicAttributes;
using Oratoria.Domain.Devices.Heater.HeaterAttributes;
using Oratoria.Domain.Devices.Magnetron.MagnetronAttributes;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module3.Signals
{
    public class Module3DI : IEnumerable<InputSignal<bool>>
    {
        private IInputStrategy<bool> _strategy;

        public ObservableCollection<InputSignal<bool>> DigitalInputs;
        public InputSignal<bool> Nakal_est { get; set; }

        public InputSignal<bool> Upravlenie_EVM { get; set; }

        public InputSignal<bool> Uroven_EVM { get; set; }


        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetron1)]
        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetron2)]
        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetron3)]
        public InputSignal<bool> Dvizhenie_BPM { get; set; }

        public InputSignal<bool> Anod_vklyuchen { get; set; }

        public InputSignal<bool> VCH_vklyuchen { get; set; }

        public InputSignal<bool> VCH_vyklyuchen { get; set; }

        public InputSignal<bool> UURG_vklyucheno { get; set; }


        [HeaterIsPowerOnSignal<Heaters>(Heaters.Heater)]
        public InputSignal<bool> BPN_vklyuchen { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetron1)]
        public InputSignal<bool> BPM1_vklyuchen { get; set; }


        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetron1)]
        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetron2)]
        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetron3)]
        public InputSignal<bool> Peregrev_BPM_est { get; set; }


        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetron1)]
        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetron2)]
        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetron3)]
        public InputSignal<bool> Peregruzka_BPM_est { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetron2)]
        public InputSignal<bool> BPM2_vklyuchen { get; set; }


        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Poziciya_1 { get; set; }


        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Poziciya_2 { get; set; }


        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Poziciya_3 { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetron3)]
        public InputSignal<bool> BPM3_vklyuchen { get; set; }


        [MechanicReversInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicReversInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicReversInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Revers_vklyuchen { get; set; }


        [CryogenicPumpIsOnSignal<Pumps>(Pumps.CryogenicPump)]
        public InputSignal<bool> Kriogennyj_nasos_vklyuchen { get; set; }


        [LeakerIsOpenSignal<Leakers>(Leakers.ArgonLeaker)]
        public InputSignal<bool> Natekatel_1_vklyuchen { get; set; }


        [LeakerIsOpenSignal<Leakers>(Leakers.NitrogenLeaker)]
        public InputSignal<bool> Natekatel_2_vklyuchen { get; set; }


        public InputSignal<bool> BP_UOG_vklyuchen { get; set; }


        [ValveIsOpenSignal<Valves>(Valves.ForValveCryoPump)]
        public InputSignal<bool> FK_KN_DU_63_otkryt { get; set; }


        [ValveIsCloseSignal<Valves>(Valves.ForValveCryoPump)]
        public InputSignal<bool> FK_KN_DU_63_zakryt { get; set; }


        [FlapIsOpenSignal<Flaps>(Flaps.Flap)]
        public InputSignal<bool> Zaslonka_otkryta { get; set; }


        [FlapIsCloseSignal<Flaps>(Flaps.Flap)]
        public InputSignal<bool> Zaslonka_zakryta { get; set; }


        [ShutterIsOpenSignal<Shutters>(Shutters.Shutter)]
        public InputSignal<bool> SHCHZ_otkryt { get; set; }


        [ShutterIsCloseSignal<Shutters>(Shutters.Shutter)]
        public InputSignal<bool> SHCHZ_zakryt { get; set; }


        public InputSignal<bool> Peregrev_vody_est { get; set; }

        public InputSignal<bool> Voda_est { get; set; }


        [MechanicTormosInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicTormosInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicTormosInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Tormoz_vklyuchen { get; set; }


        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Peregruz_privoda_est { get; set; }

        public Module3DI(ModbusTCPConfig netConfig, IInputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Nakal_est = new InputSignal<bool>("Накал есть", 0, _strategy);
            Upravlenie_EVM = new InputSignal<bool>("Управление ЭВМ", 1, _strategy);
            Uroven_EVM = new InputSignal<bool>("Уровень ЭВМ", 2, _strategy);
            Dvizhenie_BPM = new InputSignal<bool>("Движение БПМ", 3, _strategy);
            Anod_vklyuchen = new InputSignal<bool>("Анод включен", 4, _strategy);
            VCH_vklyuchen = new InputSignal<bool>("ВЧ включен", 5, _strategy);
            VCH_vyklyuchen = new InputSignal<bool>("ВЧ выключен", 6, _strategy);
            UURG_vklyucheno = new InputSignal<bool>("УУРГ включено", 7, _strategy);
            BPN_vklyuchen = new InputSignal<bool>("БПН включен", 8, _strategy);
            BPM1_vklyuchen = new InputSignal<bool>("БПМ1 включен", 9, _strategy);
            Peregrev_BPM_est = new InputSignal<bool>("Перегрев БПМ есть", 10, _strategy);
            Peregruzka_BPM_est = new InputSignal<bool>("Перегрузка БПМ есть", 11, _strategy);
            BPM2_vklyuchen = new InputSignal<bool>("БПМ2 включен", 12, _strategy);
            Poziciya_1 = new InputSignal<bool>("Позиция 1", 13, _strategy);
            Poziciya_2 = new InputSignal<bool>("Позиция 2", 14, _strategy);
            Poziciya_3 = new InputSignal<bool>("Позиция 3", 15, _strategy);
            BPM3_vklyuchen = new InputSignal<bool>("БПМ3 включен", 16, _strategy);
            Revers_vklyuchen = new InputSignal<bool>("Реверс включен", 17, _strategy);
            Kriogennyj_nasos_vklyuchen = new InputSignal<bool>("Криогенный насос включен", 18, _strategy);
            Natekatel_1_vklyuchen = new InputSignal<bool>("Натекатель 1 включен", 19, _strategy);
            Natekatel_2_vklyuchen = new InputSignal<bool>("Натекатель 2 включен", 20, _strategy);
            BP_UOG_vklyuchen = new InputSignal<bool>("БП УОГ включен", 21, _strategy);
            FK_KN_DU_63_otkryt = new InputSignal<bool>("ФК КН открыт", 22, _strategy);
            FK_KN_DU_63_zakryt = new InputSignal<bool>("ФК КН закрыт ", 23, _strategy);
            Zaslonka_otkryta = new InputSignal<bool>("Заслонка открыта", 24, _strategy);
            Zaslonka_zakryta = new InputSignal<bool>("Заслонка закрыта ", 25, _strategy);
            SHCHZ_otkryt = new InputSignal<bool>("ЩЗ открыт ", 26, _strategy);
            SHCHZ_zakryt = new InputSignal<bool>("ЩЗ закрыт ", 27, _strategy);
            Peregrev_vody_est = new InputSignal<bool>("Перегрев воды есть ", 28, _strategy);
            Voda_est = new InputSignal<bool>("Вода есть ", 29, _strategy);
            Tormoz_vklyuchen = new InputSignal<bool>("Тормоз включен ", 30, _strategy);
            Peregruz_privoda_est = new InputSignal<bool>("Перегруз привода есть ", 31, _strategy);

            DigitalInputs =
            [
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
