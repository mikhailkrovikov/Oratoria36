using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Door.DoorAttributes;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Door
{
    public class Door : OpenableDevice
    {
        public Door(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOpenSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(DoorIsOpenSignalAttribute<>));
            IsCloseSignal = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(DoorIsCloseSignalAttribute<>));
            OpenSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(DoorOpenSignalAttribute<>));
            CloseSignal = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(DoorrCloseSignalAttribute<>));

            IsOpenSignal?.OnSignalChanged += _ => OnStateChanged();
            IsCloseSignal?.OnSignalChanged += _ => OnStateChanged();
            OpenSignal?.OnSignalChanged += _ => OnStateChanged();
            CloseSignal?.OnSignalChanged += _ => OnStateChanged();
        }

        [Obsolete]
        public override Task<bool> CloseValve(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("В данном устройстве закрытие не предусмотрено");
        }

        [Obsolete]
        public override Task<bool> OpenValve(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("В данном устройстве открытие не предусмотрено");
        }
    }
}
