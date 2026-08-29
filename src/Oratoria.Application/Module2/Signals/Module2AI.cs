using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module2.Signals
{
    public class Module2AI : IEnumerable<InputSignal<double>>
    {
        IInputStrategy<double> _strategy;

        public ObservableCollection<InputSignal<double>> AnalogInputs;
        public InputSignal<double> BPNVoltage { get; set; }
        public InputSignal<double> BPNCurrent { get; set; }
        public InputSignal<double> BPM1Current { get; set; }
        public InputSignal<double> BPM1Voltage { get; set; }
        public InputSignal<double> BPM2Current { get; set; }
        public InputSignal<double> BPM2Voltage { get; set; }
        public InputSignal<double> BPM3Current { get; set; }
        public InputSignal<double> BPM3Voltage { get; set; }
        public InputSignal<double> BPNTemperature { get; set; }
        public InputSignal<double> VICB { get; set; }
        public InputSignal<double> RRGRealvalue { get; set; }
        public Module2AI(ModbusTCPConfig netConfig, IInputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            BPNVoltage = new InputSignal<double>("Напряжение БПН", 0, _strategy);
            BPNCurrent = new InputSignal<double>("Ток БПН", 1, _strategy);
            BPM1Current = new InputSignal<double>("Ток БПМ1", 2, _strategy);
            BPM1Voltage = new InputSignal<double>("Напряжение БПМ1", 3, _strategy);
            BPM2Current = new InputSignal<double>("Ток БПМ2", 4, _strategy);
            BPM2Voltage = new InputSignal<double>("Напряжение БПМ2", 5, _strategy);
            BPM3Current = new InputSignal<double>("Ток БПМ3", 6, _strategy);
            BPM3Voltage = new InputSignal<double>("Напряжение БПМ3", 7, _strategy);
            BPNTemperature = new InputSignal<double>("Температура БПН", 8, _strategy);
            VICB = new InputSignal<double>("ВИЦБ", 9, _strategy);
            RRGRealvalue = new InputSignal<double>("Расход газа: текущее", 10, _strategy);

            AnalogInputs =
            [
                BPNVoltage,
                BPNCurrent,
                BPM1Current,
                BPM1Voltage,
                BPM2Current,
                BPM2Voltage,
                BPM3Current,
                BPM3Voltage,
                BPNTemperature,
                VICB,
                RRGRealvalue
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
