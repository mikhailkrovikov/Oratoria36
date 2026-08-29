using Oratoria.Application.Module2.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.Abstractions.MechanicAttributes;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Devices.Shutter.ShutterAttributes;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module2.Signals
{

    public class Module2DO : IEnumerable<OutputSignal<bool>>
    {
        IOutputStrategy<bool> _strategy;

        public ObservableCollection<OutputSignal<bool>> DigitalOutputs;
        public OutputSignal<bool> MagnetronsRotation { get; set; }
        public OutputSignal<bool> ResetVacuumetrAlarm { get; set; }
        public OutputSignal<bool> ControlOfVacuum { get; set; }
        public OutputSignal<bool> VacuumDecontamination { get; set; }
        public OutputSignal<bool> Termopara_vklyuchit { get; set; }
        public OutputSignal<bool> BPNOn { get; set; }
        public OutputSignal<bool> BPM1On { get; set; }
        public OutputSignal<bool> BPM2On { get; set; }
        public OutputSignal<bool> BPM3On { get; set; }

        [LeakerOpenSignal<Leakers>(Leakers.ArgonLeaker)]
        public OutputSignal<bool> Leaker1On { get; set; }


        [LeakerOpenSignal<Leakers>(Leakers.NitrogenLeaker)]
        public OutputSignal<bool> Leaker2On { get; set; }

        public OutputSignal<bool> BpUogOn { get; set; }
        public OutputSignal<bool> UurgOn { get; set; }
        public OutputSignal<bool> Driver3On { get; set; }

        [ValveOpenSignal<Valves>(Valves.ForValveCryoPump)]
        public OutputSignal<bool> ForValveCryoPumpOpen { get; set; }


        [FlapCloseSignal<Flaps>(Flaps.Flap)]
        public OutputSignal<bool> FlapClose { get; set; }


        [ShutterOpenSignal<Shutters>(Shutters.Shutter)]
        public OutputSignal<bool> ShutterOpen { get; set; }


        public OutputSignal<bool> PodduvOn { get; set; }


        [MechanicDriverOutputSignal<Mechanics>(Mechanics.Manipulator)]
        public OutputSignal<bool> Driver1On { get; set; }


        [MechanicDriverOutputSignal<Mechanics>(Mechanics.Table)]
        public OutputSignal<bool> Driver2On { get; set; }


        [MechanicDriverOutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> Driver4On { get; set; }


        [MechanicPosition1OutputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition1OutputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition1OutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> Position1 { get; set; }


        [MechanicPosition2OutputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition2OutputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition2OutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> Position2 { get; set; }


        [MechanicPosition3OutputSignal<Mechanics>(Mechanics.Table)]
        [MechanicPosition3OutputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicPosition3OutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> Position3 { get; set; }


        [MechanicReversOutputSignal<Mechanics>(Mechanics.Table)]
        [MechanicReversOutputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicReversOutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> ReversOn { get; set; }


        [MechanicTormosOutputSignal<Mechanics>(Mechanics.Table)]
        [MechanicTormosOutputSignal<Mechanics>(Mechanics.Manipulator)]
        [MechanicTormosOutputSignal<Mechanics>(Mechanics.Throttle)]
        public OutputSignal<bool> TormosOn { get; set; }
        public OutputSignal<bool> CryoPumpOn { get; set; }

        public Module2DO(ModbusTCPConfig netConfig, IOutputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            MagnetronsRotation = new OutputSignal<bool>("Вращение магнетронов", 0, _strategy);
            ResetVacuumetrAlarm = new OutputSignal<bool>("Сброс аварии вакууметра", 1, _strategy);          
            ControlOfVacuum = new OutputSignal<bool>("Контроль загаживания вакуума", 3, _strategy);
            VacuumDecontamination = new OutputSignal<bool>("Обезгаживание вакуума", 4, _strategy);
            Termopara_vklyuchit = new OutputSignal<bool>("Термопара включить", 5, _strategy);
            BPNOn = new OutputSignal<bool>("БПН включить", 10, _strategy);
            BPM1On = new OutputSignal<bool>("БПМ1 включить", 11, _strategy);
            BPM2On = new OutputSignal<bool>("БПМ2 включить", 12, _strategy);
            BPM3On = new OutputSignal<bool>("БПМ3 включить", 13, _strategy);
            Leaker1On = new OutputSignal<bool>("Натекатель 1 включить", 14, _strategy);
            Leaker2On = new OutputSignal<bool>("Натекатель 2 включить", 15, _strategy);
            BpUogOn = new OutputSignal<bool>("БП УОГ включить", 16, _strategy);
            UurgOn = new OutputSignal<bool>("УУРГ включить", 17, _strategy);
            Driver3On = new OutputSignal<bool>("Привод 3 включить", 18, _strategy);
            ForValveCryoPumpOpen = new OutputSignal<bool>("ФК КН открыть", 19, _strategy);
            FlapClose = new OutputSignal<bool>("Заслонка закрыть", 20, _strategy);
            ShutterOpen = new OutputSignal<bool>("ЩЗ открыть", 21, _strategy);
            PodduvOn = new OutputSignal<bool>("Поддув включить (затвор крионасоса)", 22, _strategy);
            Driver1On = new OutputSignal<bool>("Привод 1 включить", 23, _strategy);
            Driver2On = new OutputSignal<bool>("Привод 2 включить", 24, _strategy);
            Driver4On = new OutputSignal<bool>("Привод 4 включить", 25, _strategy);
            Position1 = new OutputSignal<bool>("Позиция 1", 26, _strategy);
            Position2 = new OutputSignal<bool>("Позиция 2", 27, _strategy);
            Position3 = new OutputSignal<bool>("Позиция 3", 28, _strategy);
            ReversOn = new OutputSignal<bool>("Реверс включить", 29, _strategy);
            TormosOn = new OutputSignal<bool>("Тормоз включить", 30, _strategy);
            CryoPumpOn = new OutputSignal<bool>("Криогенный насос включить", 31, _strategy);

            DigitalOutputs =
            [
                MagnetronsRotation,
                ResetVacuumetrAlarm, 
                ControlOfVacuum,
                VacuumDecontamination,
                Termopara_vklyuchit,
                BPNOn,
                BPM1On,
                BPM2On,
                BPM3On,
                Leaker1On,
                Leaker2On,
                BpUogOn,
                UurgOn,
                Driver3On,
                ForValveCryoPumpOpen,
                FlapClose,
                ShutterOpen,
                PodduvOn,
                Driver1On,
                Driver2On,
                Driver4On,
                Position1,
                Position2,
                Position3,
                ReversOn,
                TormosOn,
                CryoPumpOn
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
