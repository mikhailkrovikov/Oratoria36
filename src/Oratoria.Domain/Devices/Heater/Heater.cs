using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;

namespace Oratoria.Domain.Devices.Heater
{
    public class Heater : Device<PowerDeviceStatus, PowerDeviceErrors>
    {

        public InputSignal<bool>? IsPowerOn { get; set; }

        public InputSignal<bool>? IsPowerOff { get; set; }

        public OutputSignal<bool>? PowerOn { get; set; }

        public OutputSignal<bool>? PowerOff { get; set; }

        public InputSignal<double>? HeaterCurrent { get; set; }

        public InputSignal<double>? HeaterVoltage { get; set; }

        public InputSignal<double>? HeaterTemp { get; set; }

        public OutputSignal<double>? HeaterPowerSetPoint { get; set; }


        public double CalculatedPower
        {
            get => Math.Round(HeaterCurrent.Value * HeaterVoltage.Value, 2);
        }


        public double CalcTemperature
        {
            get => Math.Round(HeaterTemp.Value / 10 * 400, 2);
        }

        public override PowerDeviceStatus State
        {
            get
            {
                if (IsPowerOn.Value != PowerOn.Value)
                    return PowerDeviceStatus.Transition;
                else if (IsPowerOn.Value && PowerOn.Value)
                    return PowerDeviceStatus.On;
                else if (!IsPowerOn.Value && !PowerOn.Value)
                    return PowerDeviceStatus.Off;
                return PowerDeviceStatus.Transition;
            }
        }

        public Heater(Enum deviceId, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
        }

    }
}
