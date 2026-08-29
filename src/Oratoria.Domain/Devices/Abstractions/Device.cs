using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;

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

        protected CancellationTokenSource CTSource { get; private set; } = new();

        protected Device(Enum deviceId, ILoggerFactory loggerFactory)
        {
            DeviceId = deviceId;
            DeviceErrors = new DeviceError<TError>();
            Logger = loggerFactory.CreateLogger(deviceId.GetDescription());
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
