using Modbus.Device;
using Oratoria36.Models.Connection;
using Oratoria36.Models.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules.Module2
{
    public class Module2AO
    {
        public ObservableCollection<OutputSignal<ushort>> AnalogOutputs;
        public OutputSignal<ushort> Moshchnost_BPN { get; set; }
        public OutputSignal<ushort> Moshchnost_BPM1 { get; set; }
        public OutputSignal<ushort> Moshchnost_BPM2 { get; set; }
        public OutputSignal<ushort> Moshchnost_BPM3 { get; set; }
        public OutputSignal<ushort> Upravlenie_natekatelem { get; set; }
        public OutputSignal<ushort> Raskhod_gasa_ustavka { get; set; }

        public Module2AO(NetConfig netConfig)
        {
            Moshchnost_BPN = new OutputSignal<ushort>("Мощность БПН", 0, netConfig);
            Moshchnost_BPM1 = new OutputSignal<ushort>("Мощность БПМ1", 1, netConfig);
            Moshchnost_BPM2 = new OutputSignal<ushort>("Мощность БПМ2", 2, netConfig);
            Moshchnost_BPM3 = new OutputSignal<ushort>("Мощность БПМ3", 3, netConfig);
            Upravlenie_natekatelem = new OutputSignal<ushort>("Управление натекателем", 4, netConfig);
            Raskhod_gasa_ustavka = new OutputSignal<ushort>("Расход газа: уставка", 5, netConfig);

            AnalogOutputs = new ObservableCollection<OutputSignal<ushort>>()
            {
                Moshchnost_BPN,
                Moshchnost_BPM1,
                Moshchnost_BPM2,
                Moshchnost_BPM3,
                Upravlenie_natekatelem,
                Raskhod_gasa_ustavka
            };
        }
    }
}
