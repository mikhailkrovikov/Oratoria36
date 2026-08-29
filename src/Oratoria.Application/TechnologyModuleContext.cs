using Microsoft.Extensions.Logging;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Heater;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Magnetron;
using Oratoria.Domain.Devices.Manipulator;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Table;
using Oratoria.Domain.Devices.Throttle;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Application
{
    public abstract class TechnologyModuleContext : ModuleContext
    {
        public Valve FK_KN_DU_63 { get; set; }

        public Shutter Shutter { get; set; }

        public Flap Flap { get; set; }

        public Leaker ArgonLeaker { get; set; }

        public Leaker NitrogenLeaker { get; set; }

        public Manipulator Manipulator { get; set; }

        public Throttle Throttle { get; set; }

        public Table Table { get; set; }

        public Heater Heater { get; set; }

        public Magnetron Magnetron1 { get; set; }

        public Magnetron Magnetron2 { get; set; }

        public Magnetron Magnetron3 { get; set; }

        protected TechnologyModuleContext(IModuleSignals signals, ILoggerFactory loggerFactory) : base(signals, loggerFactory)
        {
        }
    }
}
