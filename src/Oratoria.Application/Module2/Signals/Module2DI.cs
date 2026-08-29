using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.Shutter.ShutterAttributes;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Application.Module2.DeviceCollection;
using System.Collections;
using System.Collections.ObjectModel;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Devices.Abstractions.MechanicAttributes;
using Oratoria.Domain.Devices.Heater.HeaterAttributes;
using Oratoria.Domain.Devices.Magnetron.MagnetronAttributes;
using Oratoria.Domain.Devices.CryogenicPump;

namespace Oratoria.Application.Module2.Signals
{
    public class Module2DI : IEnumerable<InputSignal<bool>>
    {   
        private readonly IInputStrategy<bool> _strategy;

        public ObservableCollection<InputSignal<bool>> DigitalInputs;

        public InputSignal<bool> UurgIsOn { get; set; }

        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetrn1)]
        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetrn2)]
        [MagnetronIsRotatingSignal<Magnetrons>(Magnetrons.Magnetrn3)]
        public InputSignal<bool> Rotation_BPM { get; set; }


        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetrn1)]
        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetrn2)]
        [MagnetronOverheatSignal<Magnetrons>(Magnetrons.Magnetrn3)]
        public InputSignal<bool> BPMOverHeat { get; set; }


        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetrn1)]
        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetrn2)]
        [MagnetronOverloadSignal<Magnetrons>(Magnetrons.Magnetrn3)]
        public InputSignal<bool> BPMNoOverload { get; set; }


        [HeaterIsPowerOnSignal<Heaters>(Heaters.Heater)]
        public InputSignal<bool> BPNIsOn { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetrn1)]
        public InputSignal<bool> BPM1IsOn { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetrn2)]
        public InputSignal<bool> BPM2IsOn { get; set; }


        [MagnetronIsPowerOnSignal<Magnetrons>(Magnetrons.Magnetrn3)]
        public InputSignal<bool> BPM3IsOn { get; set; }


        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition1InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Position1 { get; set; }


        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition2InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Position2 { get; set; }


        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition3InputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Position3 { get; set; }


        [MechanicReversInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicReversInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicReversInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Revers { get; set; }


        [MechanicTormosInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicTormosInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicTormosInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> Tormos { get; set; }


        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Table)]
        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicDriverOverloadInputSignal<Mechanics>(Mechanics.Throttle)]
        public InputSignal<bool> DriverOverload { get; set; }


        [CryogenicPumpIsOnSignal<Pumps>(Pumps.CryogenicPump)]
        public InputSignal<bool> CryoPumpIsOn { get; set; }


        [LeakerIsOpenSignal<Leakers>(Leakers.ArgonLeaker)]
        public InputSignal<bool> Leaker1IsOn { get; set; }


        [LeakerIsOpenSignal<Leakers>(Leakers.NitrogenLeaker)]
        public InputSignal<bool> Leaker2IsOn { get; set; }

        public InputSignal<bool> BpUogIsOn { get; set; }


        [ValveIsOpenSignal<Valves>(Valves.ForValveCryoPump)]
        public InputSignal<bool> ForValveCryoPumpIsOpen { get; set; }


        [ValveIsCloseSignal<Valves>(Valves.ForValveCryoPump)]
        public InputSignal<bool> ForValveCryoPumpIsClose { get; set; }


        [FlapIsOpenSignal<Flaps>(Flaps.Flap)]
        public InputSignal<bool> FlapIsOpen { get; set; }


        [FlapIsCloseSignal<Flaps>(Flaps.Flap)]
        public InputSignal<bool> FlapIsClose { get; set; }


        [ShutterIsOpenSignal<Shutters>(Shutters.Shutter)]
        public InputSignal<bool> ShutterIsOpen { get; set; }


        [ShutterIsCloseSignal<Shutters>(Shutters.Shutter)]
        public InputSignal<bool> ShutterIsClose { get; set; }


        public InputSignal<bool> WaterNoOverheat { get; set; }
        public InputSignal<bool> IsWater { get; set; }




        public Module2DI(ModbusTCPConfig netConfig, IInputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Rotation_BPM = new InputSignal<bool>("Вращение магнетронов", 3, _strategy);
            UurgIsOn = new InputSignal<bool>("УУРГ включено", 7, _strategy);
            BPNIsOn = new InputSignal<bool>("БПН включен", 8, _strategy);
            BPM1IsOn = new InputSignal<bool>("БПМ1 включен", 9, _strategy);
            BPMOverHeat = new InputSignal<bool>("Перегрев БПМ есть", 10, _strategy);
            BPMNoOverload = new InputSignal<bool>("Нет перегруза БПМ", 11, _strategy);
            BPM2IsOn = new InputSignal<bool>("БПМ2 включен", 12, _strategy);
            Position1 = new InputSignal<bool>("Позиция 1", 13, _strategy);
            Position2 = new InputSignal<bool>("Позиция 2", 14, _strategy);
            Position3 = new InputSignal<bool>("Позиция 3", 15, _strategy);
            BPM3IsOn = new InputSignal<bool>("БПМ3 включен", 16, _strategy);
            Revers = new InputSignal<bool>("Реверс включен", 17, _strategy);
            CryoPumpIsOn = new InputSignal<bool>("Криогенный насос включен", 18, _strategy);
            Leaker1IsOn = new InputSignal<bool>("Натекатель 1 включен", 19, _strategy);
            Leaker2IsOn = new InputSignal<bool>("Натекатель 2 включен", 20, _strategy);
            BpUogIsOn = new InputSignal<bool>("БП УОГ включен", 21, _strategy);
            ForValveCryoPumpIsOpen = new InputSignal<bool>("ФК КН открыт", 22, _strategy);
            ForValveCryoPumpIsClose = new InputSignal<bool>("ФК КН закрыт", 23, _strategy);
            FlapIsOpen = new InputSignal<bool>("Заслонка открыта", 24, _strategy);
            FlapIsClose = new InputSignal<bool>("Заслонка закрыта", 25, _strategy);
            ShutterIsOpen = new InputSignal<bool>("ЩЗ открыт", 26, _strategy);
            ShutterIsClose = new InputSignal<bool>("ЩЗ закрыт", 27, _strategy);
            WaterNoOverheat = new InputSignal<bool>("Нет перегрева воды", 28, _strategy);
            IsWater = new InputSignal<bool>("Вода есть", 29, _strategy);
            Tormos = new InputSignal<bool>("Тормоз включен", 30, _strategy);
            DriverOverload = new InputSignal<bool>("Перегруз привода есть", 31, _strategy);

            DigitalInputs =
            [
                Rotation_BPM,
                UurgIsOn,
                BPNIsOn,
                BPM1IsOn,
                BPMOverHeat,
                BPMNoOverload,
                BPM2IsOn,
                Position1,
                Position2,
                Position3,
                BPM3IsOn,
                Revers,
                CryoPumpIsOn,
                Leaker1IsOn,
                Leaker2IsOn,
                BpUogIsOn,
                ForValveCryoPumpIsOpen,
                ForValveCryoPumpIsClose,
                FlapIsOpen,
                FlapIsClose,
                ShutterIsOpen,
                ShutterIsClose,
                WaterNoOverheat,
                IsWater,
                Tormos,
                DriverOverload,

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
