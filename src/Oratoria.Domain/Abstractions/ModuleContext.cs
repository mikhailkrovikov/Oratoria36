using Microsoft.Extensions.Logging;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Abstractions
{
    public abstract class ModuleContext
    {
        protected DeviceFactory Factory { get; set; }

        protected ModuleContext(IModuleSignals signals, ILoggerFactory loggerFactory)
        {
            Factory = new DeviceFactory(signals, loggerFactory);
        }
    }
}