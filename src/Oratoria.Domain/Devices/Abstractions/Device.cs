using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class Device<TStatus, TError> : IDevice<TStatus, TError>
        where TStatus : Enum
        where TError : Enum
    {
        public event IDevice<TStatus, TError>.StateChangeHandler? StateChanged;

        public Enum DeviceId { get; set; }

        public string DeviceName { get => DeviceId.GetDescription(); }

        public abstract TStatus? State { get; }

        public DeviceError<TError> DeviceErrors { get; set; }

        protected ILogger Logger { get; set; }

        protected ISettingsContext Settings { get; }

        protected CancellationTokenSource CTSource { get; private set; } = new();

        protected Device(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
        {
            DeviceId = deviceId;
            DeviceErrors = new DeviceError<TError>();
            Logger = loggerFactory.CreateLogger(deviceId.GetDescription());
            Settings = settings;
        }

        protected void OnStateChanged()
        {
            StateChanged?.Invoke();
        }

        protected void ResetToken()
        {
            try
            {
                CTSource.Cancel();
                CTSource.Dispose();
            }
            catch { }
            CTSource = new CancellationTokenSource();
        }
    }
}
