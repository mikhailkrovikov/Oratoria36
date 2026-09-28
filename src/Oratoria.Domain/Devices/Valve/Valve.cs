using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Devices.Abstractions;

namespace Oratoria.Domain.Devices.Valve
{
    public class Valve : OpenableDevice
    {
        public Valve(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOpenSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ValveIsOpenSignalAttribute<>));
            IsCloseSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ValveIsCloseSignalAttribute<>));
            OpenSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ValveOpenSignalAttribute<>))!;
            CloseSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ValveCloseSignalAttribute<>));

            IsOpenSignal?.OnSignalChanged += _ => OnStateChanged();
            IsCloseSignal?.OnSignalChanged += _ => OnStateChanged();
            OpenSignal?.OnSignalChanged += _ => OnStateChanged();
            CloseSignal?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
