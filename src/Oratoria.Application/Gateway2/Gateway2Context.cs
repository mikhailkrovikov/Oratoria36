using Microsoft.Extensions.Logging;
using Oratoria.Application.Gateway1;
using Oratoria.Application.Gateway2.DeviceCollections;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Door;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Settings;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Gateway2
{
    public class Gateway2Context : GatewayContext
    {
        public override string Name => ModuleId.Gateway2.GetDescription();

        public Gateway2Context(TransportSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            Door = Factory.CreateDevice<Door>(Doors.Door2);
            Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl2Shutter);
        }
    }
}
