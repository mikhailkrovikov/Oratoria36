using Microsoft.Extensions.Logging;
using Oratoria.Application.Gateway1.DeviceCollection;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Door;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Settings;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Gateway1
{
    public class GatewayContext : ModuleContext
    {
        public override string Name { get; }
        public Door Door { get; set; }
        public Shutter Shutter { get; set; }
        public GatewayContext(TransportSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {

        }
    }

    public class Gateway1Context : GatewayContext
    {
        public Gateway1Context(TransportSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            Door = Factory.CreateDevice<Door>(Doors.Door1);
            Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl1Shutter);
        }

        public override string Name => ModuleId.Gateway1.GetDescription();
    }
}
