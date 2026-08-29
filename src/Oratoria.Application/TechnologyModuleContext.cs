using Microsoft.Extensions.Logging;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Heater;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Magnetron;
using Oratoria.Domain.Devices.Manipulator;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Domain.Devices.RRG;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Table;
using Oratoria.Domain.Devices.Throttle;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Application
{
    public abstract class TechnologyModuleContext : ModuleContext
    {
        public Valve FK_KN_DU_63 { get; set; } = null!;

        public Shutter Shutter { get; set; } = null!;

        public Flap Flap { get; set; } = null!;

        public Leaker ArgonLeaker { get; set; } = null!;

        public Leaker NitrogenLeaker { get; set; } = null!;

        public RRG RRG{ get; set; } = null!;

        public Manipulator Manipulator { get; set; } = null!;

        public Throttle Throttle { get; set; } = null!;

        public Table Table { get; set; } = null!;

        public Heater Heater { get; set; } = null!;

        public Magnetron Magnetron1 { get; set; } = null!;

        public Magnetron Magnetron2 { get; set; } = null!;

        public Magnetron Magnetron3 { get; set; } = null!;

        public CryogenicPump CryogenicPump { get; set; } = null!;

        public HighVacuumSensor VICB { get; set; } = null!;

        protected TechnologyModuleContext(IModuleSignals signals, ILoggerFactory loggerFactory) : base(signals, loggerFactory)
        {
        }
    }
}
