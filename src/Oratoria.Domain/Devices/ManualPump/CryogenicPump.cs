using Microsoft.Extensions.Logging;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.ManualPump
{
    public class CryogenicPump(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : ManualPump(deviceId, signals, loggerFactory)
    {
    }
}
