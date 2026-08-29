using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Leaker
{
    public class Leaker : OpenableDevice
    {
        public OutputSignal<double>? LeakerSetpoint { get; set; }

        public Leaker(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
            IsOpen = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsOpenSignalAttribute<>));
            IsClose = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsCloseSignalAttribute<>));
            Open = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapOpenSignalAttribute<>));
            Close = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapCloseSignalAttribute<>));
            LeakerSetpoint = SignalHelper<OutputSignal<double>>.GetSignal(deviceId, signals.AOSignals, typeof(FlapSetpointSignalAttribute<>));

            IsOpen?.OnSignalChanged += _ => OnStateChanged();
            IsClose?.OnSignalChanged += _ => OnStateChanged();
            Open?.OnSignalChanged += _ => OnStateChanged();
            Close?.OnSignalChanged += _ => OnStateChanged();
        }

        public async Task<bool> SetConsumption(double percent)
        {
            LeakerSetpoint?.Value = percent / 10;
            if (State != OpenableStatus.Open)
                return await OpenValve();
            return true;
        }
    }
}
