using Modbus.Device;
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

        public Module2AO(ModbusIpMaster master)
        {
            Moshchnost_BPN = new OutputSignal<ushort>("Мощность БПН", 0, master);
            Moshchnost_BPM1 = new OutputSignal<ushort>("Мощность БПМ1", 1, master);
            Moshchnost_BPM2 = new OutputSignal<ushort>("Мощность БПМ2", 2, master);
            Moshchnost_BPM3 = new OutputSignal<ushort>("Мощность БПМ3", 3, master);
            Upravlenie_natekatelem = new OutputSignal<ushort>("Управление натекателем", 4, master);
            Raskhod_gasa_ustavka = new OutputSignal<ushort>("Расход газа: уставка", 5, master);

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
