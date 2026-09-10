using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Microsoft.Extensions.Logging;
using Oratoria.Application.Module3.DeviceCollection;
using Oratoria.Application.Module3.Signals;
using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Manipulator;
using Oratoria.Domain.Devices.Table;
using Oratoria.Domain.Devices.Throttle;
using Oratoria.Domain.Devices.Magnetron;
using Oratoria.Domain.Devices.Heater;
using Oratoria.Domain.Devices.RRG;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Settings;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Module3
{
    public class Module3Context : TechnologyModuleContext
    {
        public Module3Context(Module3Signals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            FK_KN_DU_63 = Factory.CreateDevice<Valve>(Valves.ForValveCryoPump);
            Shutter = Factory.CreateDevice<Shutter>(Shutters.Shutter);
            Flap = Factory.CreateDevice<Flap>(Flaps.Flap);
            ArgonLeaker = Factory.CreateDevice<Leaker>(Leakers.ArgonLeaker);
            NitrogenLeaker = Factory.CreateDevice<Leaker>(Leakers.NitrogenLeaker);
            RRG = Factory.CreateDevice<RRG>(RRGs.RRG);
            Manipulator = Factory.CreateDevice<Manipulator>(Mechanics.Manipulator);
            Throttle = Factory.CreateDevice<Throttle>(Mechanics.Throttle);
            Table = Factory.CreateDevice<Table>(Mechanics.Table);
            Heater = Factory.CreateDevice<Heater>(Heaters.Heater);
            Magnetron1 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetron1);
            Magnetron2 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetron2);
            Magnetron3 = Factory.CreateDevice<Magnetron>(Magnetrons.Magnetron3);
            CryogenicPump = Factory.CreateDevice<CryogenicPump>(Pumps.CryogenicPump);
            VICB = Factory.CreateDevice<HighVacuumSensor>(PressureSensors.VICB);
        }

        public override string Name => ModuleId.Module3.GetDescription();
    }
}
