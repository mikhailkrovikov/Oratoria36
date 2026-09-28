using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Leaker.LeakerAttributes;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Leaker
{
    public class Leaker : OpenableDevice
    {
        public OutputSignal<double>? LeakerSetpoint { get; set; }

        public Leaker(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOpenSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(LeakerIsOpenSignalAttribute<>));
            IsCloseSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(LeakerIsCloseSignalAttribute<>));
            OpenSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(LeakerOpenSignalAttribute<>));
            CloseSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(LeakerCloseSignalAttribute<>));
            LeakerSetpoint = SignalHelper<OutputSignal<double>>.GetSignal(deviceId, signals.AOSignals, typeof(LeakerSetpointSignalAttribute<>));

            IsOpenSignal?.OnSignalChanged += _ => OnStateChanged();
            IsCloseSignal?.OnSignalChanged += _ => OnStateChanged();
            OpenSignal?.OnSignalChanged += _ => OnStateChanged();
            CloseSignal?.OnSignalChanged += _ => OnStateChanged();
        }

        [DeviceAction("Уставка")]
        public async Task<bool> SetConsumption([DeviceActionParameter("%")] double percent, CancellationToken cancellationToken = default)
        {
            LeakerSetpoint?.Value = percent / 10;
            if (State != OpenableStatus.Open)
                return await OpenValve(cancellationToken);
            return true;
        }
    }
}
