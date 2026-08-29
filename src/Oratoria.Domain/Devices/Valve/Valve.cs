using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Valve.ValveAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Devices.Abstractions;

namespace Oratoria.Domain.Devices.Valve
{
    public class Valve : OpenableDevice
    {
        public Valve(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
            IsOpen = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ValveIsOpenSignalAttribute<>));
            IsClose = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ValveIsCloseSignalAttribute<>));
            Open = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ValveOpenSignalAttribute<>))!;
            Close = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(ValveCloseSignalAttribute<>));

            IsOpen?.OnSignalChanged += _ => OnStateChanged();
            IsClose?.OnSignalChanged += _ => OnStateChanged();
            Open?.OnSignalChanged += _ => OnStateChanged();
            Close?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
