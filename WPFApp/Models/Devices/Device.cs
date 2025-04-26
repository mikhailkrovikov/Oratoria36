using Oratoria36.Models.Signals;
using Oratoria36.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Devices
{
    public abstract class Device
    {
        public abstract State State { get; }
        public InputSignal<bool>? IsOn { get; }
        public InputSignal<bool>? IsOff { get; }
        public OutputSignal<bool>? On { get; }
        public OutputSignal<bool>?  Off { get; }
    }
}
