using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Shutter.ShutterAttributes;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Shutter
{
    public class Shutter : OpenableDevice
    {
        public Shutter(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOpen = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ShutterIsOpenSignalAttribute<>));
            IsClose = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ShutterIsCloseSignalAttribute<>));
            Open = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ShutterOpenSignalAttribute<>));
            Close = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ShutterCloseSignalAttribute<>));

            IsOpen?.OnSignalChanged += _ => OnStateChanged();
            IsClose?.OnSignalChanged += _ => OnStateChanged();
            Open?.OnSignalChanged += _ => OnStateChanged();
            Close?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
