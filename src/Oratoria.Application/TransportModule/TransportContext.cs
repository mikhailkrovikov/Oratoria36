using Microsoft.Extensions.Logging;
using Oratoria.Application.TransportModule.DeviceCollection;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Door;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Application.TransportModule
{
    public class TransportContext : ModuleContext
    {
        public Door Door1 { get; set; }

        public Door Door2 { get; set; }

        public Shutter Shl1Shutter { get; set; }

        public Shutter Shl2Shutter { get; set; }

        public TransportContext(TransportSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            Shl1Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl1Shutter);
            Shl2Shutter = Factory.CreateDevice<Shutter>(Shutters.Shl2Shutter);
            Door1 = Factory.CreateDevice<Door>(Doors.Door1);
            Door2 = Factory.CreateDevice<Door>(Doors.Door2);
        }
    }
}
