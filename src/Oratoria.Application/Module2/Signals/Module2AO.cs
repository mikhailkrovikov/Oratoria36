using Oratoria.Application.Module2.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module2.Signals
{
    public class Module2AO : IEnumerable<OutputSignal<double>>
    {
        IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;
        public OutputSignal<double> BPNPower { get; set; }
        public OutputSignal<double> BPM1Power { get; set; }
        public OutputSignal<double> BPM2Power { get; set; }
        public OutputSignal<double> BPM3Power { get; set; }


        [LeakerSetpointSignal<Leakers>(Leakers.ArgonLeaker)]
        public OutputSignal<double> LeakerControl { get; set; }


        public OutputSignal<double> RRGSetpoint { get; set; }

        public Module2AO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            BPNPower = new OutputSignal<double>("Мощность БПН", 0, _strategy);
            BPM1Power = new OutputSignal<double>("Мощность БПМ1", 1, _strategy);
            BPM2Power = new OutputSignal<double>("Мощность БПМ2", 2, _strategy);
            BPM3Power = new OutputSignal<double>("Мощность БПМ3", 3, _strategy);
            LeakerControl = new OutputSignal<double>("Управление натекателем", 4, _strategy);
            RRGSetpoint = new OutputSignal<double>("Расход газа: уставка", 5, _strategy);

            AnalogOutputs =
            [
                BPNPower,
                BPM1Power,
                BPM2Power,
                BPM3Power,
                LeakerControl,
                RRGSetpoint
            ];
        }

        public IEnumerator<OutputSignal<double>> GetEnumerator()
        {
            return AnalogOutputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
