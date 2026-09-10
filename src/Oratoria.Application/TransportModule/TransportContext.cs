using Microsoft.Extensions.Logging;
using Oratoria.Application.TransportModule.DeviceCollection;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.Carriage;
using Oratoria.Domain.Devices.Door;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Application.TransportModule
{
    public class TransportContext : ModuleContext
    {
        public Carriage Carriage { get; set; }

        public TransportContext(TransportSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            Carriage = Factory.CreateDevice<Carriage>(Mechanics.Сarriage);
        }

        public override string Name => ModuleId.TransportModule.GetDescription();
    }
}
