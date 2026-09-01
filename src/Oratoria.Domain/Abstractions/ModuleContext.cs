using Microsoft.Extensions.Logging;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Abstractions
{
    public abstract class ModuleContext
    {
        protected DeviceFactory Factory { get; }

        protected ModuleContext(IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
        {
            Factory = new DeviceFactory(signals, loggerFactory, settings);
        }
    }
}