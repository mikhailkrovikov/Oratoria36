using Oratoria.Application.VacuumModule.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;
namespace Oratoria.Application.VacuumModule.Signals
{
    public class VacuumDI: IEnumerable<InputSignal<bool>>
    {
        private readonly IInputStrategy<bool> _strategy;

        public ObservableCollection<InputSignal<bool>> DigitalInputs;


        [ValveIsOpenSignal<Valves>(Valves.FK_AVR)]
        public InputSignal<bool> FK_AVR_Opened {  get; set; }


        [ValveIsCloseSignal<Valves>(Valves.FK_AVR)]
        public InputSignal<bool> FK_AVR_Closed {  get; set; }
        public InputSignal<bool> FK_AP_Opened {  get; set; }
        public InputSignal<bool> FK_AP_Closed {  get; set; }
        public InputSignal<bool> FK_OK_Opened {  get; set; }
        public InputSignal<bool> FK_OK_Closed {  get; set; }
        public InputSignal<bool> FK_KN1_Opened {  get; set; }
        public InputSignal<bool> FK_KN1_Closed {  get; set; }
        public InputSignal<bool> FK_KN2_Opened { get; set; }
        public InputSignal<bool> FK_KN2_Closed { get; set; }
        public InputSignal<bool> OilPump_On { get; set; }
        public InputSignal<bool> RUTSPump_On { get; set; }
        public InputSignal<bool> FK_M1_Opened { get; set; }
        public InputSignal<bool> FK_M1_Closed { get; set; }
        public InputSignal<bool> FK_M2_Opened { get; set; }
        public InputSignal<bool> FK_M2_Closed { get; set; }
        public InputSignal<bool> FK_M3_Opened { get; set; }
        public InputSignal<bool> FK_M3_Closed { get; set; }
        public InputSignal<bool> FK_M4_Opened { get; set; }
        public InputSignal<bool> FK_M4_Closed { get; set; }
        public InputSignal<bool> FK_TM_Opened { get; set; }
        public InputSignal<bool> FK_TM_Closed { get; set; }
        public InputSignal<bool> FK_Shl1_Opened { get; set; }
        public InputSignal<bool> FK_Shl1_Closed { get; set; }
        public InputSignal<bool> FK_Shl2_Opened { get; set; }
        public InputSignal<bool> FK_Shl2_Closed { get; set; }
        public InputSignal<bool> KN_Zatvor_TM_Opened { get; set; }
        public InputSignal<bool> KN_Zatvor_TM_Closed { get; set; }
        public InputSignal<bool> KN1_On { get; set; }
        public InputSignal<bool> KN2_On { get; set; }
        public InputSignal<bool> AP1_On { get; set; }
        public InputSignal<bool> AP2_On { get; set; }
        public InputSignal<bool> FK_Trb_Opened { get; set; }
        public InputSignal<bool> FK_Trb_Closed { get; set; }
        public VacuumDI(ModbusTCPConfig netConfig, IInputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            FK_AVR_Opened = new InputSignal<bool>("ФК АВР открыт", 24, _strategy);
            FK_AVR_Closed = new InputSignal<bool>("ФК АВР закрыт", 25, _strategy);
            FK_AP_Opened = new InputSignal<bool>("ФК азотного питателя открыт", 26, _strategy);
            FK_AP_Closed = new InputSignal<bool>("ФК азотного питателя закрыт", 27, _strategy);
            FK_OK_Opened = new InputSignal<bool>("Обводной клапан открыт", 28, _strategy);
            FK_OK_Closed = new InputSignal<bool>("Обводной клапан закрыт", 29, _strategy);
            FK_KN1_Opened = new InputSignal<bool>("ФК КН1 (транспорт) открыт", 30, _strategy);
            FK_KN1_Closed = new InputSignal<bool>("ФК КН1 (транспорт) закрыт", 31, _strategy);
            FK_KN2_Opened = new InputSignal<bool>("Затвор КН2 (шлюзы) открыт", 32, _strategy);
            FK_KN2_Closed = new InputSignal<bool>("Затвор КН2 (шлюзы) закрыт", 33, _strategy);
            OilPump_On = new InputSignal<bool>("Масляный насос включен", 34, _strategy);
            RUTSPump_On = new InputSignal<bool>("Насос Рутса включен", 35, _strategy);
            FK_M1_Opened = new InputSignal<bool>("ФК модуля 1 открыт", 36, _strategy);
            FK_M1_Closed = new InputSignal<bool>("ФК модуля 1 закрыт", 37, _strategy);
            FK_M2_Opened = new InputSignal<bool>("ФК модуля 2 открыт", 38, _strategy);
            FK_M2_Closed = new InputSignal<bool>("ФК модуля 2 закрыт", 39, _strategy);
            FK_M3_Opened = new InputSignal<bool>("ФК модуля 3 открыт", 40, _strategy);
            FK_M3_Closed = new InputSignal<bool>("ФК модуля 3 закрыт", 41, _strategy);
            FK_M4_Opened = new InputSignal<bool>("ФК модуля 4 открыт", 42, _strategy);
            FK_M4_Closed = new InputSignal<bool>("ФК модуля 4 закрыт", 43, _strategy);
            FK_TM_Opened = new InputSignal<bool>("ФК трансп. модуля открыт", 44, _strategy);
            FK_TM_Closed = new InputSignal<bool>("ФК трансп. модуля закрыт", 45, _strategy);
            FK_Shl1_Opened = new InputSignal<bool>("ФК шлюза 1 открыт", 46, _strategy);
            FK_Shl1_Closed = new InputSignal<bool>("ФК шлюза 1 закрыт", 47, _strategy);
            FK_Shl2_Opened = new InputSignal<bool>("ФК шлюза 2 открыт", 48, _strategy);
            FK_Shl2_Closed = new InputSignal<bool>("ФК шлюза 1 закрыт", 49, _strategy);
            KN_Zatvor_TM_Opened = new InputSignal<bool>("Затвор КН1 (транспорт) открыт", 50, _strategy);
            KN_Zatvor_TM_Closed = new InputSignal<bool>("Затвор КН1 (транспорт) закрыт", 51, _strategy);
            KN1_On = new InputSignal<bool>("КН1 (транспорт) включен", 52, _strategy);
            KN2_On = new InputSignal<bool>("КН2 (шлюзы) включен", 53, _strategy);
            AP1_On = new InputSignal<bool>("Азотный питатель 1 включен", 54, _strategy);
            AP2_On = new InputSignal<bool>("Азотный питатель 2 включен", 55, _strategy);
            FK_Trb_Opened = new InputSignal<bool>("ФК трубопровода открыт", 56, _strategy);
            FK_Trb_Closed = new InputSignal<bool>("ФК трубопровода закрыт", 57, _strategy);

            DigitalInputs =
            [
                FK_AVR_Opened,
                FK_AVR_Closed,
                FK_AP_Opened,
                FK_AP_Closed,
                FK_OK_Opened,
                FK_OK_Closed,
                FK_KN1_Opened,
                FK_KN1_Closed,
                FK_KN2_Opened,
                FK_KN2_Closed,
                OilPump_On,
                RUTSPump_On,
                FK_M1_Opened,
                FK_M1_Closed,
                FK_M2_Opened,
                FK_M2_Closed,
                FK_M3_Opened,
                FK_M3_Closed,
                FK_M4_Opened,
                FK_M4_Closed,
                FK_TM_Opened,
                FK_TM_Closed,
                FK_Shl1_Opened,
                FK_Shl1_Closed,
                FK_Shl2_Opened,
                FK_Shl2_Closed,
                KN_Zatvor_TM_Opened,
                KN_Zatvor_TM_Closed,
                KN1_On,
                KN2_On,
                AP1_On,
                AP2_On,
                FK_Trb_Opened,
                FK_Trb_Closed,
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
