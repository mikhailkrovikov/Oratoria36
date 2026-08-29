using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class PowerDevice : Device<PowerDeviceStatus, PowerDeviceErrors>
    {
        protected InputSignal<bool>? IsPowerOn { get; set; }

        protected InputSignal<bool>? IsPowerOff { get; set; }

        protected OutputSignal<bool>? PowerOn { get; set; }

        protected OutputSignal<bool>? PowerOff { get; set; }

        protected Setting<int> TimeForError { get; }

        public override PowerDeviceStatus State
        {
            get
            {
                if (IsPowerOn != null && IsPowerOff != null)
                {
                    if (IsPowerOff.Value == IsPowerOn.Value)
                        return PowerDeviceStatus.Transition;
                    if (IsPowerOff.Value)
                        return PowerDeviceStatus.Off;
                    if (IsPowerOn.Value)
                        return PowerDeviceStatus.On;
                    return PowerDeviceStatus.Transition;
                }
                if (IsPowerOn != null)
                {
                    if (IsPowerOn.Value)
                        return PowerDeviceStatus.On;
                    return PowerDeviceStatus.Off;
                }
                if (IsPowerOff != null)
                {
                    if (IsPowerOff.Value)
                        return PowerDeviceStatus.Off;
                    return PowerDeviceStatus.On;
                }
                if (PowerOn != null)
                {
                    if (PowerOn.Value)
                        return PowerDeviceStatus.On;
                    return PowerDeviceStatus.Off;
                }
                if (PowerOff != null)
                {
                    if (PowerOff.Value)
                        return PowerDeviceStatus.Off;
                    return PowerDeviceStatus.On;
                }
                return PowerDeviceStatus.Transition;
            }
        }

        protected PowerDevice(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
            TimeForError = new(DeviceId, "Время до ошибки", "сек");
        }

        public virtual async Task<bool> TurnOn()
        {
            Logger.LogInformation($"{DeviceName}: включение");
            ResetToken();
            var token = CTSource.Token;
            try
            {
                if (State == PowerDeviceStatus.On)
                {
                    DeviceErrors.ResetError(PowerDeviceErrors.CannotTurnOn);
                    return true;
                }

                PowerOn?.Value = true;
                PowerOff?.Value = false;

                var res = true;
                var needWait = false;

                if (IsPowerOn != null && !IsPowerOn.Value)
                    needWait = true;

                if (IsPowerOff != null && IsPowerOff.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    if (IsPowerOn != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsPowerOn.OnSignalChanged),
                            IsPowerOn,
                            (bool x) =>
                            {
                                if (IsPowerOn != null)
                                    return IsPowerOn.Value;

                                if (IsPowerOff != null)
                                    return !IsPowerOff.Value;

                                return true;
                            },
                            TimeForError.Value * 1000, token);
                    }
                    else if (IsPowerOff != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsPowerOff.OnSignalChanged),
                            IsPowerOff,
                            (bool x) => !IsPowerOff.Value,
                            TimeForError.Value * 1000, token);
                    }
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: не смог включиться, авария");
                    DeviceErrors.AddError(PowerDeviceErrors.CannotTurnOn);
                    PowerOn?.Value = false;
                    return false;
                }
                DeviceErrors.ResetError(PowerDeviceErrors.CannotTurnOn);
                IsPowerOn?.OnSignalChanged += IsPowerOn_OnSignalChanged;
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
                if (State == PowerDeviceStatus.Off)
                {
                    DeviceErrors.ResetError(PowerDeviceErrors.CannotTurnOff);
                    return true;
                }

                IsPowerOn?.OnSignalChanged -= IsPowerOn_OnSignalChanged;
                PowerOn?.Value = false;
                PowerOff?.Value = true;

                var res = true;
                var needWait = false;

                if (IsPowerOff != null && !IsPowerOff.Value)
                    needWait = true;

                if (IsPowerOn != null && IsPowerOn.Value)
                    needWait = true;

                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    if (IsPowerOff != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsPowerOff.OnSignalChanged),
                            IsPowerOff,
                            (bool x) =>
                            {
                                if (IsPowerOff != null)
                                    return IsPowerOff.Value;

                                if (IsPowerOn != null)
                                    return !IsPowerOn.Value;

                                return true;
                            },
                            TimeForError.Value * 1000, token);
                    }
                    else if (IsPowerOn != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsPowerOn.OnSignalChanged),
                            IsPowerOn,
                            (bool x) => !IsPowerOn.Value,
                            TimeForError.Value * 1000, token);
                    }
                }
                if (!res)
                {
                    Logger.LogError($"{DeviceName}: не смог выключиться, авария");
                    DeviceErrors.AddError(PowerDeviceErrors.CannotTurnOff);
                    PowerOff?.Value = false;
                    return false;
                }
                DeviceErrors.ResetError(PowerDeviceErrors.CannotTurnOff);
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

        private void IsPowerOn_OnSignalChanged(bool value)
        {
            if (!value)
            {
                Logger.LogError($"{DeviceName}: произошло неожиданное выключение");
                DeviceErrors.AddError(PowerDeviceErrors.UnexpectedShutDown);
            }
        }
    }
}