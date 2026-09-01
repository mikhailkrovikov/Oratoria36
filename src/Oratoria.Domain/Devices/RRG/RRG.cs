using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.RRG.RRGAttributes;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.RRG
{
    public class RRG : Device<RRGStatus, RRGErrors>
    {
        public InputSignal<double> RRGRealValueSignal { get; set; }

        public OutputSignal<double> RRGSetpointSignal { get; set; }

        public Setting<double> RRGDifference { get; set; }

        public Setting<double> MaxFlowRate { get; set; }

        public Setting<int> TimeOfAction { get; set; }

        public double RRGRealValue
        {
            get => Math.Round(RRGRealValueSignal.Value * (MaxFlowRate.Value / 5), 2);
        }

        public double RRGSetPointValue
        {
            get => Math.Round(RRGSetpointSignal.Value * (MaxFlowRate.Value / 5), 2);
        }

        public override RRGStatus State
        {
            get
            {
                if (RRGSetPointValue == 0)
                {
                    if (RRGRealValue == 0)
                        return RRGStatus.Close;
                    return RRGStatus.Transition;
                }
                var deviation = Math.Abs(RRGSetPointValue - RRGRealValue)
                                / RRGSetPointValue * 100;
                if (deviation <= RRGDifference.Value)
                    return RRGStatus.Open;
                return RRGStatus.Transition;
            }
        }

        public RRG(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            RRGRealValueSignal = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(RRGRealValueSignalAttribute<>));
            RRGSetpointSignal = SignalHelper<OutputSignal<double>>.GetSignal(deviceId, signals.AOSignals, typeof(RRGSetpointSignalAttribute<>));

            RRGDifference = Settings.GetSetting(deviceId, nameof(RRGDifference), "Предел отклонения", "%", 5.0, 0.0, 100.0);
            MaxFlowRate = Settings.GetSetting(deviceId, nameof(MaxFlowRate), "Верхний предел", "л/ч", 100.0);
            TimeOfAction = Settings.GetSetting(deviceId, nameof(TimeOfAction), "Время выхода уставку", "сек", 30);
        }

        public async Task<bool> SetValue(double value)
        {
            if (MaxFlowRate.Value == 0)
            {
                Logger.LogWarning($"{DeviceName}: не задан предел расхода РРГ");
                return false;
            }

            if (value == 0)
                return await ResetValue();

            Logger.LogInformation($"{DeviceName}: выход на уставку {value} л/ч");
            ResetToken();
            var token = CTSource.Token;
            RRGRealValueSignal.OnSignalChanged -= CheckState;
            try
            {
                RRGSetpointSignal.Value = value * (5.0 / MaxFlowRate.Value);
                if (State != RRGStatus.Open)
                {
                    var result = await EventWaiter.WaitEvent
                    (nameof(RRGRealValueSignal.OnSignalChanged),
                    RRGRealValueSignal,
                    (double x) => State == RRGStatus.Open, TimeOfAction.Value * 1000, token);

                    if (!result)
                    {
                        Logger.LogWarning($"{DeviceName}: не удается достичь уставки");
                        DeviceErrors.AddError(RRGErrors.CannotSetCons);
                        return false;
                    }
                    else
                    {
                        DeviceErrors.ResetError(RRGErrors.CannotSetCons);
                        RRGRealValueSignal.OnSignalChanged += CheckState;
                        return true;
                    }
                }
                DeviceErrors.ResetError(RRGErrors.CannotSetCons);
                RRGRealValueSignal.OnSignalChanged += CheckState;
                return true;

            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"{DeviceName}: открытие отменено");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка открытия");
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> ResetValue()
        {
            RRGRealValueSignal.OnSignalChanged -= CheckState;
            Logger.LogInformation($"{DeviceName}: сброс уставки");
            ResetToken();
            var token = CTSource.Token;

            try
            {
                RRGSetpointSignal.Value = 0;
                if (State != RRGStatus.Close)
                {
                    var result = await EventWaiter.WaitEvent
                    (nameof(RRGRealValueSignal.OnSignalChanged),
                    RRGRealValueSignal,
                    (double x) => State == RRGStatus.Close, TimeOfAction.Value * 1000, token);

                    if (!result)
                    {
                        Logger.LogWarning($"{DeviceName}: не удается сбросить уставки");
                        DeviceErrors.AddError(RRGErrors.CannotResetCons);
                        return false;
                    }
                    else
                    {
                        DeviceErrors.ResetError(RRGErrors.CannotResetCons);
                        return true;
                    }
                }
                DeviceErrors.ResetError(RRGErrors.CannotResetCons);
                return true;
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"{DeviceName}: закрытие отменено");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка закрытия");
                Logger.LogError(ex.Message);
                return false;
            }
        }

        private void CheckState(double value)
        {
            if (State != RRGStatus.Open)
                Logger.LogWarning($"{DeviceName}: выход из нормального режима работы");
        }
    }
}
