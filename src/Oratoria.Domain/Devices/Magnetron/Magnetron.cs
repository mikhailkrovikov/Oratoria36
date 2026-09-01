using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Magnetron.MagnetronAttributes;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Magnetron
{
    public class Magnetron : PowerDevice
    {
        public InputSignal<bool> IsRotating { get; set; }

        public InputSignal<bool> MagnetronOverload { get; set; }

        public InputSignal<bool> MagnetronOverheat { get; set; }

        public InputSignal<double> MagnetronCurrent { get; set; }

        public InputSignal<double> MagnetronVoltage { get; set; }

        public OutputSignal<bool> RotationOn { get; set; }

        public OutputSignal<double> MagnetronPowerSetPoint { get; set; }

        public double CalculatedPower
        {
            get => Math.Round(MagnetronCurrent.Value * MagnetronVoltage.Value, 2);
        }

        public Magnetron(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsPowerOff = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MagnetronIsPowerOffSignalAttribute<>));
            IsPowerOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MagnetronIsPowerOnSignalAttribute<>));
            IsRotating = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MagnetronIsRotatingSignalAttribute<>))!;
            MagnetronOverheat = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MagnetronOverheatSignalAttribute<>))!;
            MagnetronOverload = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MagnetronOverloadSignalAttribute<>))!;
            PowerOff = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MagnetronPowerOffSignalAttribute<>));
            PowerOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MagnetronPowerOnSignalAttribute<>));
            RotationOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MagnetronRotatingSignalAttribute<>))!;
            MagnetronCurrent = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(MagnetronCurrentSignalAttribute<>))!;
            MagnetronVoltage = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(MagnetronVoltageSignalAttribute<>))!;
            MagnetronPowerSetPoint = SignalHelper<OutputSignal<double>>.GetSignal(deviceId, signals.AOSignals, typeof(MagnetronSetpointSignalAttribute<>))!;

            IsPowerOff?.OnSignalChanged += _ => OnStateChanged();
            IsPowerOn?.OnSignalChanged += _ => OnStateChanged();
            PowerOff?.OnSignalChanged += _ => OnStateChanged();
            PowerOn?.OnSignalChanged += _ => OnStateChanged();

            MagnetronOverheat?.OnSignalChanged += value => Magnetron_OnSignalChanged(value, PowerDeviceErrors.Overheat);
            MagnetronOverload?.OnSignalChanged += value => Magnetron_OnSignalChanged(!value, PowerDeviceErrors.Overload);
        }

        private void Magnetron_OnSignalChanged(bool value, PowerDeviceErrors error)
        {
            if (value)
            {
                DeviceErrors.AddError(error);
                Logger.LogWarning($"{DeviceName}: {error.GetDescription()}");
            }
            else
                DeviceErrors.ResetError(error);

        }

        public virtual async Task<bool> TurnOn(double setpoint)
        {
            var result = await base.TurnOn();
            if (result)
                MagnetronPowerSetPoint.Value = setpoint;
            return result;
        }

        public virtual async Task<bool> ResetSetpoint()
        {
            MagnetronPowerSetPoint.Value = 0;
            return await base.TurnOff();
        }
    }

}
