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
            IsOpenSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ShutterIsOpenSignalAttribute<>));
            IsCloseSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ShutterIsCloseSignalAttribute<>));
            OpenSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ShutterOpenSignalAttribute<>));
            CloseSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ShutterCloseSignalAttribute<>));

            IsOpenSignal?.OnSignalChanged += _ => OnStateChanged();
            IsCloseSignal?.OnSignalChanged += _ => OnStateChanged();
            OpenSignal?.OnSignalChanged += _ => OnStateChanged();
            CloseSignal?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
