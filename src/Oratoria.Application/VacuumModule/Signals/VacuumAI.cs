using Oratoria.Application.VacuumModule.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.PressureSensor.PressureSensorAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.VacuumModule.Signals
{
    public  class VacuumAI : IEnumerable<InputSignal<double>>
    {
        private readonly IInputStrategy<double> _strategy;

        public ObservableCollection<InputSignal<double>> AnalogInputs;


        [PressureSensorSignal<PressureSensors>(PressureSensors.AVRLowVacuum)]
        public InputSignal<double> AVR_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Gateway1LowVacuum)]
        public InputSignal<double> Shl1_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Gateway2LowVacuum)]
        public InputSignal<double> Shl2_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.TransportLowVacuum)]
        public InputSignal<double> Transport_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.TransportHighVacuum)]
        public InputSignal<double> Transport_VV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Module1LowPressure)]
        public InputSignal<double> Module1_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Module2LowPressure)]
        public InputSignal<double> Module2_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Module3LowPressure)]
        public InputSignal<double> Module3_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.Module4LowPressure)]
        public InputSignal<double> Module4_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.KNTransportLowVacuum)]
        public InputSignal<double> KN1_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.KNTransportHighVacuum)]
        public InputSignal<double> KN1_VV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.KNGatewaytLowVacuum)]
        public InputSignal<double> KN2_NV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.KNGatewayHighVacuum)]
        public InputSignal<double> KN2_VV {  get; set; }


        [PressureSensorSignal<PressureSensors>(PressureSensors.TrupoprovodLowVacuum)]
        public InputSignal<double> Trupoprovod_NV {  get; set; }

        public VacuumAI(ModbusTCPConfig netConfig, IInputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            AVR_NV = new InputSignal<double>("АВР: низкий вауум", 2, _strategy);
            Shl1_NV = new InputSignal<double>("Шлюз 1: низкий вакуум", 3, _strategy);
            Shl2_NV = new InputSignal<double>("Шлюз 2: низкий вакуум", 4, _strategy);
            Transport_NV = new InputSignal<double>("Транспорт: низкий вакуум", 5, _strategy);
            Transport_VV = new InputSignal<double>("Транспорт: высокий вакуум", 6, _strategy);
            Module1_NV = new InputSignal<double>("Модуль 1: низкий вакуум", 7, _strategy);
            Module2_NV = new InputSignal<double>("Модуль 2: низкий вакуум", 8, _strategy);
            Module3_NV = new InputSignal<double>("Модуль 3: низкий вакуум", 9, _strategy);
            Module4_NV = new InputSignal<double>("Модуль 4: низкий вакуум", 10, _strategy);
            KN1_NV = new InputSignal<double>("КН1 (транспорт): низкий вакуум", 11, _strategy);
            KN1_VV = new InputSignal<double>("КН1 (транспорт): высокий вакуум", 12, _strategy);
            KN2_NV = new InputSignal<double>("КН2 (шлюзы): низкий вакуум", 13, _strategy);
            KN2_VV = new InputSignal<double>("КН2 (шлюзы): высокий вакуум", 14, _strategy);
            Trupoprovod_NV = new InputSignal<double>("Трубопровод: низкий вакуум", 15, _strategy);

            AnalogInputs =
            [
                AVR_NV,
                Shl1_NV,
                Shl2_NV,
                Transport_NV,
                Transport_VV,
                Module1_NV,
                Module2_NV,
                Module3_NV,
                Module4_NV,
                KN1_NV,
                KN1_VV,
                KN2_NV,
                KN2_VV,
                Trupoprovod_NV,
            ];
        }

        public IEnumerator<InputSignal<double>> GetEnumerator()
        {
            return AnalogInputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
