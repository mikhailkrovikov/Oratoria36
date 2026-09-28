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
            IsOpenSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsOpenSignalAttribute<>));
            IsCloseSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(FlapIsCloseSignalAttribute<>));
            OpenSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapOpenSignalAttribute<>));
            CloseSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(FlapCloseSignalAttribute<>));

            IsOpenSignal?.OnSignalChanged += _ => OnStateChanged();
            IsCloseSignal?.OnSignalChanged += _ => OnStateChanged();
            OpenSignal?.OnSignalChanged += _ => OnStateChanged();
            CloseSignal?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
