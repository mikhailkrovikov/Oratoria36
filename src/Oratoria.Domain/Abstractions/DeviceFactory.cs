using Microsoft.Extensions.Logging;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Abstractions
{
    public class DeviceFactory
    {
        private readonly IModuleSignals _signals;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ISettingsContext _settings;

        public DeviceFactory(IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
        {
            _signals = signals;
            _loggerFactory = loggerFactory;
            _settings = settings;
        }

        public TDevice CreateDevice<TDevice>(Enum deviceId) where TDevice : class
        {
            var device = Activator.CreateInstance(typeof(TDevice), deviceId, _signals, _loggerFactory, _settings);
            if (device == null)
                throw new Exception("Unable to create device, check enums");        
            return (TDevice)device;
        }
    }
}