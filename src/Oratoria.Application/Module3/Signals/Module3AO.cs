using Oratoria.Application.Module3.DeviceCollection;
using Oratoria.Domain.Devices.Heater.HeaterAttributes;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Devices.Magnetron.MagnetronAttributes;
using Oratoria.Domain.Devices.RRG.RRGAttributes;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Module3.Signals
{
    public class Module3AO : IEnumerable<OutputSignal<double>>
    {
        IOutputStrategy<double> _strategy;

        public ObservableCollection<OutputSignal<double>> AnalogOutputs;


        [HeaterSetpointSignal<Heaters>(Heaters.Heater)]
        public OutputSignal<double> Moshchnost_BPN { get; set; }


        [MagnetronSetpointSignal<Magnetrons>(Magnetrons.Magnetron1)]
        public OutputSignal<double> Moshchnost_BPM1 { get; set; }


        [MagnetronSetpointSignal<Magnetrons>(Magnetrons.Magnetron2)]
        public OutputSignal<double> Moshchnost_BPM2 { get; set; }


        [MagnetronSetpointSignal<Magnetrons>(Magnetrons.Magnetron3)]
        public OutputSignal<double> Moshchnost_BPM3 { get; set; }


        [LeakerSetpointSignal<Leakers>(Leakers.ArgonLeaker)]
        public OutputSignal<double> Upravlenie_natekatelem { get; set; }


        [RRGSetpointSignal<RRGs>(RRGs.RRG)]
        public OutputSignal<double> Raskhod_gasa_ustavka { get; set; }


        public Module3AO(ModbusTCPConfig netConfig, IOutputStrategy<double> strategy)
        {
#if RELEASE
            _strategy = new AOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif
            Moshchnost_BPN = new OutputSignal<double>("Мощность БПН", 0, _strategy);
            Moshchnost_BPM1 = new OutputSignal<double>("Мощность БПМ1", 1, _strategy);
            Moshchnost_BPM2 = new OutputSignal<double>("Мощность БПМ2", 2, _strategy);
            Moshchnost_BPM3 = new OutputSignal<double>("Мощность БПМ3", 3, _strategy);
            Upravlenie_natekatelem = new OutputSignal<double>("Управление натекателем", 4, _strategy);
            Raskhod_gasa_ustavka = new OutputSignal<double>("Расход газа: уставка", 5, _strategy);

            AnalogOutputs =
            [
                Moshchnost_BPN,
                Moshchnost_BPM1,
                Moshchnost_BPM2,
                Moshchnost_BPM3,
                Upravlenie_natekatelem,
                Raskhod_gasa_ustavka
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
