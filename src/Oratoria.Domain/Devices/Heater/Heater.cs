using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Heater.HeaterAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Heater
{
    public class Heater : PowerDevice
    {
        public InputSignal<double> HeaterCurrent { get; set; }

        public InputSignal<double> HeaterVoltage { get; set; }

        public InputSignal<double> HeaterTemp { get; set; }

        public OutputSignal<double> HeaterPowerSetPoint { get; set; }

        public double CalculatedPower
        {
            get => Math.Round(HeaterCurrent.Value * HeaterVoltage.Value, 2);
        }

        public double CalcTemperature
        {
            get => Math.Round(HeaterTemp.Value / 10 * 400, 2);
        }

        public Heater(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
            IsPowerOff = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(HeaterIsPowerOffSignalAttribute<>));
            IsPowerOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(HeaterIsPowerOnSignalAttribute<>));
            PowerOff = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(HeaterPowerOffSignalAttribute<>));
            PowerOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(HeaterPowerOnSignalAttribute<>));
            HeaterCurrent = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(HeaterCurrentSignalAttribute<>));
            HeaterVoltage = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(HeaterVoltageSignalAttribute<>));
            HeaterTemp = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(HeaterTemperatureSignalAttribute<>));
            HeaterPowerSetPoint = SignalHelper<OutputSignal<double>>.GetSignal(deviceId, signals.AOSignals, typeof(HeaterSetpointSignalAttribute<>));

            IsPowerOff?.OnSignalChanged += _ => OnStateChanged();
            IsPowerOn?.OnSignalChanged += _ => OnStateChanged();
            PowerOff?.OnSignalChanged += _ => OnStateChanged();
            PowerOn?.OnSignalChanged += _ => OnStateChanged();
        }

        public virtual async Task<bool> TurnOn(double setpoint)
        {
            var result = await TurnOn();
            if (result)
                HeaterPowerSetPoint.Value = setpoint;
            return result;
        }
        public virtual async Task<bool> ResetSetpoint()
        {
            HeaterPowerSetPoint.Value = 0;
            return await TurnOff();
        }
    }
}
