using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Door.DoorAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Door
{
    public class Door : OpenableDevice
    {
        public Door(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            IsOpen = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(DoorIsOpenSignalAttribute<>));
            IsClose = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(DoorIsCloseSignalAttribute<>));
            Open = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(DoorOpenSignalAttribute<>));
            Close = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(DoorrCloseSignalAttribute<>));

            IsOpen?.OnSignalChanged += _ => OnStateChanged();
            IsClose?.OnSignalChanged += _ => OnStateChanged();
            Open?.OnSignalChanged += _ => OnStateChanged();
            Close?.OnSignalChanged += _ => OnStateChanged();
        }

        [Obsolete]
        public override Task<bool> CloseValve()
        {
            throw new NotSupportedException("В данном устройстве закрытие не предусмотрено");
        }

        [Obsolete]
        public override Task<bool> OpenValve()
        {
            throw new NotSupportedException("В данном устройстве открытие не предусмотрено");
        }
    }
}
