using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.AVRPump.AVRPumpAttributes;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.AVRPump
{
    public class AVRPump : Device<PumpStatus, PumpErrors>
    {
        public InputSignal<bool> IsOilPumpOn { get; set; }
        public InputSignal<bool> IsRutsPumpOn { get; set; }
        public OutputSignal<bool> OilPumpOn { get; set; }
        public OutputSignal<bool> RutsPumpOn { get; set; }

        public Setting<int> OilPumpTime { get; set; }
        public Setting<int> RutsPumpTime { get; set; }

        public override PumpStatus State
        {
            get
            {
                if (IsOilPumpOn.Value && IsRutsPumpOn.Value)
                    return PumpStatus.On;
                if (!IsOilPumpOn.Value && !IsRutsPumpOn.Value)
                    return PumpStatus.Off;
                return PumpStatus.Transition;
            }
        }

        public AVRPump(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            IsOilPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(AVRIsOilOnSignalAttribute<>))!;
            IsRutsPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(AVRIsRutsOnSignalAttribute<>))!;
            OilPumpOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(AVROilOnSignalAttribute<>))!;
            RutsPumpOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(AVRRutsOnSignalAttribute<>))!;

            OilPumpTime = Settings.GetSetting(deviceId, nameof(OilPumpTime), "Время включения маслянного насоса", "сек", 30);
            RutsPumpTime = Settings.GetSetting(deviceId, nameof(RutsPumpTime), "Время включения насоса РУТС", "сек", 30);

            IsOilPumpOn.OnSignalChanged += _ => OnStateChanged();
            IsRutsPumpOn.OnSignalChanged += _ => OnStateChanged();
            OilPumpOn.OnSignalChanged += _ => OnStateChanged();
            RutsPumpOn.OnSignalChanged += _ => OnStateChanged();
        }

        public virtual async Task<bool> TurnOn()
        {
            Logger.LogInformation($"{DeviceName}: включение");
            ResetToken();
            var token = CTSource.Token;
            try
            {
                if (State == PumpStatus.On)
                {
                    DeviceErrors.ResetRangeErrors(
                        PumpErrors.CannotTurnOn,
                        PumpErrors.UnexpectedOilShutDown,
                        PumpErrors.UnexpectedRutsShutDown);
                    SubscribeWatchers();
                    return true;
                }

                UnsubscribeWatchers();

                OilPumpOn.Value = true;

                var res = true;
                var needWait = false;
                if (!IsOilPumpOn.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsOilPumpOn.OnSignalChanged),
                        IsOilPumpOn,
                        (bool x) => IsOilPumpOn.Value,
                        OilPumpTime.Value * 1000, token);
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: масляный насос не включился, авария");
                    DeviceErrors.AddError(PumpErrors.CannotTurnOn);
                    return false;
                }

                IsOilPumpOn.OnSignalChanged += OnOilPumpFeedbackChanged;

                RutsPumpOn.Value = true;

                needWait = false;
                if (!IsRutsPumpOn.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsRutsPumpOn.OnSignalChanged),
                        IsRutsPumpOn,
                        (bool x) => IsRutsPumpOn.Value,
                        RutsPumpTime.Value * 1000, token);
                }
                if (!IsOilPumpOn.Value)
                {
                    if (!DeviceErrors.HasError(PumpErrors.UnexpectedOilShutDown))
                    {
                        Logger.LogError($"{DeviceName}: пропал сигнал масляного насоса, авария");
                        DeviceErrors.AddError(PumpErrors.UnexpectedOilShutDown);
                    }
                    RutsPumpOn.Value = false;
                    UnsubscribeWatchers();
                    return false;
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: насос Рутса не включился, авария");
                    DeviceErrors.AddError(PumpErrors.CannotTurnOn);
                    RutsPumpOn.Value = false;
                    UnsubscribeWatchers();
                    return false;
                }

                DeviceErrors.ResetRangeErrors(
                    PumpErrors.CannotTurnOn,
                    PumpErrors.UnexpectedOilShutDown,
                    PumpErrors.UnexpectedRutsShutDown);
                SubscribeWatchers();
                return true;
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"{DeviceName}: включение отменено");
                UnsubscribeWatchers();
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка включения");
                Logger.LogError(ex.Message);
                UnsubscribeWatchers();
                return false;
            }
        }

        public virtual async Task<bool> TurnOff()
        {
            Logger.LogInformation($"{DeviceName}: выключение");
            ResetToken();
            var token = CTSource.Token;
            try
            {
                if (State == PumpStatus.Off)
                {
                    DeviceErrors.ResetError(PumpErrors.CannotTurnOff);
                    UnsubscribeWatchers();
                    return true;
                }

                UnsubscribeWatchers();
                RutsPumpOn.Value = false;

                var res = true;
                var needWait = false;
                if (IsRutsPumpOn.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsRutsPumpOn.OnSignalChanged),
                        IsRutsPumpOn,
                        (bool x) => !IsRutsPumpOn.Value,
                        RutsPumpTime.Value * 1000, token);
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: насос Рутса не выключился, авария");
                    DeviceErrors.AddError(PumpErrors.CannotTurnOff);
                    SubscribeWatchers();
                    return false;
                }

                OilPumpOn.Value = false;

                needWait = false;
                if (IsOilPumpOn.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsOilPumpOn.OnSignalChanged),
                        IsOilPumpOn,
                        (bool x) => !IsOilPumpOn.Value,
                        OilPumpTime.Value * 1000, token);
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: масляный насос не выключился, авария");
                    DeviceErrors.AddError(PumpErrors.CannotTurnOff);
                    return false;
                }

                DeviceErrors.ResetError(PumpErrors.CannotTurnOff);
                return true;
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"{DeviceName}: выключение отменено");
                SubscribeWatchers();
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка выключения");
                Logger.LogError(ex.Message);
                SubscribeWatchers();
                return false;
            }
        }

        private void SubscribeWatchers()
        {
            UnsubscribeWatchers();
            IsOilPumpOn.OnSignalChanged += OnOilPumpFeedbackChanged;
            IsRutsPumpOn.OnSignalChanged += OnRutsPumpFeedbackChanged;
        }

        private void UnsubscribeWatchers()
        {
            IsOilPumpOn.OnSignalChanged -= OnOilPumpFeedbackChanged;
            IsRutsPumpOn.OnSignalChanged -= OnRutsPumpFeedbackChanged;
        }

        private void OnOilPumpFeedbackChanged(bool value)
        {
            if (value)
                return;

            Logger.LogError($"{DeviceName}: пропал сигнал масляного насоса, авария");
            RutsPumpOn.Value = false;
            DeviceErrors.AddError(PumpErrors.UnexpectedOilShutDown);
            UnsubscribeWatchers();
        }

        private void OnRutsPumpFeedbackChanged(bool value)
        {
            if (value)
                return;

            Logger.LogError($"{DeviceName}: пропал сигнал насоса Рутса, авария");
            RutsPumpOn.Value = false;
            DeviceErrors.AddError(PumpErrors.UnexpectedRutsShutDown);
            UnsubscribeWatchers();
        }
    }
}
