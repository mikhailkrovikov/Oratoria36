using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Flap.FlapAttributes;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Flap
{
    public class Flap : OpenableDevice
    {
        public Flap(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOpen = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsOpenSignalAttribute<>));
            IsClose = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsCloseSignalAttribute<>));
            Open = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapOpenSignalAttribute<>));
            Close = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapCloseSignalAttribute<>));

            IsOpen?.OnSignalChanged += _ => OnStateChanged();
            IsClose?.OnSignalChanged += _ => OnStateChanged();
            Open?.OnSignalChanged += _ => OnStateChanged();
            Close?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
