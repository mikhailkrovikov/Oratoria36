using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.ManualPump
{
    public class ManualPump : Device<PumpStatus, PumpErrors>
    {
        public InputSignal<bool> IsPumpOn { get; set; }

        public ManualPump(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            IsPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(ManualPumpIsOnSignalAttribute<>))!;
            IsPumpOn?.OnSignalChanged += value => 
            {
                HandleInput(value);
                OnStateChanged(); 
            };
        }

        private void HandleInput(bool value)
        {
            if (value) 
                Logger.LogInformation($"{DeviceName}: включился");         
            else
                Logger.LogInformation($"{DeviceName}: выключился");      
        }

        public override PumpStatus State
        {
            get
            {
                if (IsPumpOn.Value)
                    return PumpStatus.On;
                return PumpStatus.Off;
            }
        }
    }
}
