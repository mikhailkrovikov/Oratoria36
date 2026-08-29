using Microsoft.Extensions.Logging;
using Oratoria.Application.TransportModule.DeviceCollection;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Door;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Application.TransportModule
{
    public class TransportContext : ModuleContext
    {
        public Door Door1 { get; set; }

        public Door Door2 { get; set; }

        public Shutter Shl1Shutter { get; set; }

        public Shutter Shl2Shutter { get; set; }

        public TransportContext(IModuleSignals signals, ILoggerFactory loggerFactory) : base(signals, loggerFactory)
        {
            Shl1Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl1Shutter);
            Shl2Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl2Shutter);
            Door1 = Factory.CreateDevice<Door>(Doors.Door1);
            Door2 = Factory.CreateDevice<Door>(Doors.Door2);
        }
    }
}
