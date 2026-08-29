using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Microsoft.Extensions.Logging;
using Oratoria.Application.Module2.DeviceCollection;
using Oratoria.Application.Module2.Signals;
using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Manipulator;
using Oratoria.Domain.Devices.Table;
using Oratoria.Domain.Devices.Throttle;
using Oratoria.Domain.Devices.Magnetron;
using Oratoria.Domain.Devices.Heater;

namespace Oratoria.Application.Module2
{
    public class Module2Context : TechnologyModuleContext
    {
        public Module2Context(Module2Signals signals, ILoggerFactory loggerFactory) : base(signals, loggerFactory)
        {
            FK_KN_DU_63 = Factory.CreateDevice<Valve>(Valves.ForValveCryoPump);
            Shutter = Factory.CreateDevice<Shutter>(Shutters.Shutter);
            Flap = Factory.CreateDevice<Flap>(Flaps.Flap);
            ArgonLeaker = Factory.CreateDevice<Leaker>(Leakers.ArgonLeaker);
            NitrogenLeaker = Factory.CreateDevice<Leaker>(Leakers.NitrogenLeaker);
            Manipulator = Factory.CreateDevice<Manipulator>(Mechanics.Manipulator);
            Throttle = Factory.CreateDevice<Throttle>(Mechanics.Throttle);
            Table = Factory.CreateDevice<Table>(Mechanics.Table);
            Heater = Factory.CreateDevice<Heater>(Heaters.Heater);
            Magnetron1 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetrn1);
            Magnetron2 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetrn2);
            Magnetron3 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetrn3);
        }
    }
}
