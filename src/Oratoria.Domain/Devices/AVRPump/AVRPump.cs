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
        public InputSignal<bool> IsOilPumpOn {  get; set; }
        public InputSignal<bool> IsRutsPumpOn {  get; set; }
        public OutputSignal<bool> OilPumpOn {  get; set; }
        public OutputSignal<bool> RutsPumpOn {  get; set; }

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

        public AVRPump(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            IsOilPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(AVRIsOilOnSignalAttribute<>))!;
            IsRutsPumpOn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(AVRIsRutsOnSignalAttribute<>))!;
            OilPumpOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(AVROilOnSignalAttribute<>))!;
            RutsPumpOn = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(AVRRutsOnSignalAttribute<>))!;

            OilPumpTime = new(deviceId, "Время включения маслянного насоса", "сек");
            RutsPumpTime = new(deviceId, "Время включения насоса РУТС", "сек");

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
                    DeviceErrors.ResetError(PumpErrors.CannotTurnOn);
                    return true;
                }

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
                    OilPumpOn.Value = false;
                    return false;
                }

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
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: насос Рутса не включился, авария");
                    DeviceErrors.AddError(PumpErrors.CannotTurnOn);
                    RutsPumpOn.Value = false;
                    return false;
                }

                DeviceErrors.ResetError(PumpErrors.CannotTurnOn);
                return true;
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"{DeviceName}: включение отменено");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка включения");
                Logger.LogError(ex.Message);
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
                    return true;
                }

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
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DeviceName}: ошибка выключения");
                Logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
