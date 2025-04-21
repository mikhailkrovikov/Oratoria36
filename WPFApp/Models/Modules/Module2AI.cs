using Modbus.Device;
using Oratoria36.Models.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2AI
    {
        public ObservableCollection<InputSignal<ushort>> AnalogInputs;
        public InputSignal<ushort> Napryazhenie_BPN { get; set; }
        public InputSignal<ushort> Tok_BPN { get; set; }
        public InputSignal<ushort> Tok_BPM1 { get; set; }
        public InputSignal<ushort> Napryazhenie_BPM1 { get; set; }
        public InputSignal<ushort> Tok_BPM2 { get; set; }
        public InputSignal<ushort> Napryazhenie_BPM2 { get; set; }
        public InputSignal<ushort> Tok_BPM3 { get; set; }
        public InputSignal<ushort> Napryazhenie_BPM3 { get; set; }
        public InputSignal<ushort> Termopara { get; set; }
        public InputSignal<ushort> VICB { get; set; }
        public InputSignal<ushort> Raskhod_gasa_tekushchee { get; set; }
        public Module2AI(ModbusIpMaster master)
        {
            Napryazhenie_BPN = new InputSignal<ushort>("Напряжение БПН", 0, master);
            Tok_BPN = new InputSignal<ushort>("Ток БПН", 1, master);
            Tok_BPM1 = new InputSignal<ushort>("Ток БПМ1", 2, master);
            Napryazhenie_BPM1 = new InputSignal<ushort>("Напряжение БПМ1", 3, master);
            Tok_BPM2 = new InputSignal<ushort>("Ток БПМ2", 4, master);
            Napryazhenie_BPM2 = new InputSignal<ushort>("Напряжение БПМ2", 5, master);
            Tok_BPM3 = new InputSignal<ushort>("Ток БПМ3", 6, master);
            Napryazhenie_BPM3 = new InputSignal<ushort>("Напряжение БПМ3", 7, master);
            Termopara = new InputSignal<ushort>("Термопара", 8, master);
            VICB = new InputSignal<ushort>("ВИЦБ", 9, master);
            Raskhod_gasa_tekushchee = new InputSignal<ushort>("Расход газа: текущее", 10, master);

            AnalogInputs = new ObservableCollection<InputSignal<ushort>>()
            {
                Napryazhenie_BPN,
                Tok_BPN,
                Tok_BPM1,
                Napryazhenie_BPM1,
                Tok_BPM2,
                Napryazhenie_BPM2,
                Tok_BPM3,
                Napryazhenie_BPM3,
                Termopara,
                VICB,
                Raskhod_gasa_tekushchee
            };
        }
    }
}
