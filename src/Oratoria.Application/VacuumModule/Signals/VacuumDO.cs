using Oratoria.Application.VacuumModule.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.AVRPump.AVRPumpAttributes;
using Oratoria.Domain.Devices.NitrogenFeeder.NitrogenFeederAttributes;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.VacuumModule.Signals
{
    public class VacuumDO : IEnumerable<OutputSignal<bool>>
    {
        private IOutputStrategy<bool> _strategy;

        public ObservableCollection<OutputSignal<bool>> DigitalOutputs;

        [ValveOpenSignal<Valves>(Valves.FK_AVR)]
        public OutputSignal<bool> FK_AVR { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_AP)]
        public OutputSignal<bool> FK_AP { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_OK)]
        public OutputSignal<bool> FK_OK { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_KN1)]
        public OutputSignal<bool> FK_KN1 { get; set; }


        [ValveOpenSignal<Valves>(Valves.KN2_Zatvor)]
        public OutputSignal<bool> FK_KN2 { get; set; }


        [ValveOpenSignal<Valves>(Valves.KN_Zatvor_TM)]
        public OutputSignal<bool> KN_Zatvor_TM { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_TM)]
        public OutputSignal<bool> FK_TM { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_Shl1)]
        public OutputSignal<bool> FK_Shl1 { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_Shl2)]
        public OutputSignal<bool> FK_Shl2 { get; set; }


        [AVROilOnSignal<Pumps>(Pumps.AVR)]
        public OutputSignal<bool> OilPump { get; set; }


        [AVRRutsOnSignal<Pumps>(Pumps.AVR)]
        public OutputSignal<bool> RUTSPump { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_M1)]
        public OutputSignal<bool> FK_M1 { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_M2)]
        public OutputSignal<bool> FK_M2 { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_M3)]
        public OutputSignal<bool> FK_M3 { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_M4)]
        public OutputSignal<bool> FK_M4 { get; set; }


        [ValveOpenSignal<Valves>(Valves.FK_Trb)]
        public OutputSignal<bool> FK_Trb { get; set; }


        [NitrogenFeederPowerOnSignal<NitrogenFeeders>(NitrogenFeeders.NitrogenFeeder1)]
        public OutputSignal<bool> AP1 { get; set; }


        [NitrogenFeederPowerOnSignal<NitrogenFeeders>(NitrogenFeeders.NitrogenFeeder2)]
        public OutputSignal<bool> AP2 { get; set; }


        public OutputSignal<bool> AlarmStop { get; set; }


        [Obsolete]
        public OutputSignal<bool> KN1 { get; set; }

        [Obsolete]
        public OutputSignal<bool> KN2 { get; set; }

        public VacuumDO(ModbusTCPConfig netConfig, IOutputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            FK_AVR = new OutputSignal<bool>("ФК АВР открыть", 30, _strategy);
            FK_AP = new OutputSignal<bool>("ФК азотного питателя открыть", 31, _strategy);
            FK_OK = new OutputSignal<bool>("Обводной клапан открыть", 32, _strategy);
            FK_KN1 = new OutputSignal<bool>("ФК КН 1 (транспорт) открыть", 33, _strategy);
            FK_KN2 = new OutputSignal<bool>("Затвор КН2 (шлюзы) открыть", 34, _strategy);
            KN_Zatvor_TM = new OutputSignal<bool>("Затвор КН1 (транспорт) открыть", 35, _strategy);
            FK_TM = new OutputSignal<bool>("ФК трансп. модуля открыть", 36, _strategy);
            FK_Shl1 = new OutputSignal<bool>("ФК шлюза 1 открыть", 37, _strategy);
            FK_Shl2 = new OutputSignal<bool>("ФК шлюза 2 открыт", 38, _strategy);
            OilPump = new OutputSignal<bool>("Масляный насос включить", 39, _strategy);
            RUTSPump = new OutputSignal<bool>("Насос Рутса включить", 40, _strategy);
            FK_M1 = new OutputSignal<bool>("ФК модуля 1 открыть", 41, _strategy);
            FK_M2 = new OutputSignal<bool>("ФК модуля 2 открыть", 42, _strategy);
            FK_M3 = new OutputSignal<bool>("ФК модуля 3 открыть", 43, _strategy);
            FK_M4 = new OutputSignal<bool>("ФК модуля 4 открыть", 44, _strategy);
            KN1 = new OutputSignal<bool>("КН1 (транспорт) включить", 45, _strategy);
            KN2 = new OutputSignal<bool>("КН2 (шлюзы) включить", 46, _strategy);
            AP1 = new OutputSignal<bool>("Азотный питатель 1 включить", 47, _strategy);
            AP2 = new OutputSignal<bool>("Азотный питатель 2 включить", 48, _strategy);
            AlarmStop = new OutputSignal<bool>("Аварийный стоп", 49, _strategy);
            FK_Trb = new OutputSignal<bool>("ФК трубопровода открыть", 50, _strategy);

            DigitalOutputs =
            [
                FK_AVR,
                FK_AP,
                FK_OK,
                FK_KN1,
                FK_KN2,
                KN_Zatvor_TM,
                FK_TM,
                FK_Shl1,
                FK_Shl2,
                OilPump,
                RUTSPump,
                FK_M1,
                FK_M2,
                FK_M3,
                FK_M4,
                KN1,
                KN2,
                AP1,
                AP2,
                AlarmStop,
                FK_Trb,
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
