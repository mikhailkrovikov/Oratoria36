using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.CryogenicPump
{
    public class CryogenicPump : Device<PumpStatus, PumpErrors>
    {
        public InputSignal<bool> IsPumpOn { get; set; }

        public CryogenicPump(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            IsPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(CryogenicPumpIsOnSignalAttribute<>))!;
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
