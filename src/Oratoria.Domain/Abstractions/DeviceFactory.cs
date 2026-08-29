using Microsoft.Extensions.Logging;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Abstractions
{
    public class DeviceFactory
    {
        private readonly IModuleSignals _signals;
        private readonly ILoggerFactory _loggerFactory;
        public DeviceFactory(IModuleSignals signals, ILoggerFactory loggerFactory)
        {
            _signals = signals;
            _loggerFactory = loggerFactory;
        }

        public TDevice CreateDevice<TDevice>(Enum deviceId) where TDevice : class
        {
            var device = Activator.CreateInstance(typeof(TDevice), deviceId, _signals, _loggerFactory);
            return (TDevice)device;
        }
    }
}