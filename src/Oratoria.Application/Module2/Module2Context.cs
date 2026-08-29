using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Microsoft.Extensions.Logging;
using Oratoria.Application.Module2.DeviceCollection;
using Oratoria.Application.Module2.Signals;
using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Manipulator;

namespace Oratoria.Application.Module2
{
    public class Module2Context : ITechModuleContext
    {
        public Valve FK_KN_DU_63 { get; set; }
        public Shutter Shutter { get; set; }
        public Flap Flap { get; set; }
        public Leaker ArgonLeaker { get; set; }
        public Leaker NitrogenLeaker { get; set; }
        public Manipulator Manipulator { get; set; }

        public Module2Context(Module2Signals signals, ILoggerFactory loggerFactory)
        {
            FK_KN_DU_63 = new Valve(Valves.ForValveCryoPump, signals, loggerFactory);
            Shutter = new Shutter(Shutters.Shutter, signals, loggerFactory);
            Flap = new Flap(Flaps.Flap, signals, loggerFactory);
            ArgonLeaker = new Leaker(Leakers.ArgonLeaker, signals, loggerFactory);
            NitrogenLeaker = new Leaker(Leakers.NitrogenLeaker, signals, loggerFactory);
            Manipulator = new Manipulator(Mechanics.Manipulator, signals, loggerFactory);
        }
    }
}
