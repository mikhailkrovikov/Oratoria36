using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.NitrogenFeeder.NitrogenFeederAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.NitrogenFeeder
{
    public class NitrogenFeeder : PowerDevice
    {
        public NitrogenFeeder(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            IsPowerOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(NitrogenFeederIsPowerOnSignalAttribute<>));
            IsPowerOff = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(NitrogenFeederIsPowerOffSignalAttribute<>));
            PowerOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(NitrogenFeederPowerOnSignalAttribute<>));
            PowerOff = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(NitrogenFeederPowerOffSignalAttribute<>));

            IsPowerOff?.OnSignalChanged += _ => OnStateChanged();
            IsPowerOn?.OnSignalChanged += _ => OnStateChanged();
            PowerOff?.OnSignalChanged += _ => OnStateChanged();
            PowerOn?.OnSignalChanged += _ => OnStateChanged();
        }
    }
}
