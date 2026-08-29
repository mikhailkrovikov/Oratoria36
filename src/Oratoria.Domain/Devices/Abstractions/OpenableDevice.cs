using Oratoria.Infrastructure;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class OpenableDevice : Device<OpenableStatus, OpenableErrors>
    {
        protected InputSignal<bool>? IsOpen { get; set; }

        protected InputSignal<bool>? IsClose { get; set; }

        protected OutputSignal<bool>? Open { get; set; }

        protected OutputSignal<bool>? Close { get; set; }

        protected Setting<int> TimeForWarning { get; }

        protected Setting<int> TimeForError { get; }

        public override OpenableStatus State
        {
            get
            {
                if (IsOpen != null && IsClose != null)
                {
                    if (IsClose.Value == IsOpen.Value)
                        return OpenableStatus.Transition;
                    if (IsClose.Value)
                        return OpenableStatus.Close;
                    if (IsOpen.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Transition;
                }
                if (IsOpen != null)
                {
                    if (IsOpen.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Close;
                }
                if (IsClose != null)
                {
                    if (IsClose.Value)
                        return OpenableStatus.Close;
                    return OpenableStatus.Open;
                }
                if (Open != null)
                {
                    if (Open.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Close;
                }
                if (Close != null)
                {
                    if (Close.Value)
                        return OpenableStatus.Close;
                    return OpenableStatus.Open;
                }
                return OpenableStatus.Transition;
            }
        }

        protected OpenableDevice(Enum deviceId, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
            TimeForError = new(DeviceId, "Время до ошибки", "сек");
            TimeForWarning = new(DeviceId, "Время до предупреждения", "сек");
        }


        public virtual async Task<bool> OpenValve()
        {
            Logger.LogInformation($"{DeviceName}: открытие");
            ResetToken();
            var token = CTSource.Token;
            try
            {
                if (State == OpenableStatus.Open)
                {
                    DeviceErrors.ResetRangeErrors(OpenableErrors.CannotOpen, OpenableErrors.TooLongOpening);
                    return true;
                }

                Open?.Value = true;
                Close?.Value = false;

                var res = true;
                var needWait = false;

                if (IsOpen != null && !IsOpen.Value)
                    needWait = true;

                if (IsClose != null && IsClose.Value)
                    needWait = true;


                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    if (IsOpen != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                            IsOpen,
                            (bool x) =>
                            {
                                if (IsOpen != null)
                                    return IsOpen.Value;
                                
                                if (IsClose != null)
                                    return !IsClose.Value;
                                
                                return true;
                            },
                            TimeForWarning.Value * 1000, token);
                    }
                    else if (IsClose != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                            IsClose,
                            (bool x) => !IsClose.Value,
                            TimeForWarning.Value * 1000, token);
                    }
                }
                if (!res)
                {
                    Logger.LogWarning($"{DeviceName}: долгое открытие, предупреждение");
                    DeviceErrors.AddError(OpenableErrors.TooLongOpening);
                    if (IsOpen != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                            IsOpen,
                            (bool x) =>
                            {
                                if (IsOpen != null)
                                    return IsOpen.Value;
                                
                                if (IsClose != null)
                                    return !IsClose.Value;
                                
                                return true;
                            },
                            TimeForError.Value * 1000, token);
                    }
                    else if (IsClose != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                            IsClose,
                            (bool x) => !IsClose.Value,
                            TimeForError.Value * 1000, token);
                    }
                    if (!res)
                    {
                        Logger.LogError($"{DeviceName}: не смог открыться, авария");
                        DeviceErrors.AddError(OpenableErrors.CannotOpen);
                        Open?.Value = false;
                        return false;
                    }
                }
                DeviceErrors.ResetRangeErrors(OpenableErrors.CannotOpen, OpenableErrors.TooLongOpening);
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

        public virtual async Task<bool> CloseValve()
        {
            Logger.LogInformation($"{DeviceName}: закрытие");
            ResetToken();
            var token = CTSource.Token;
            try
            {
                if (State == OpenableStatus.Close)
                {
                    DeviceErrors.ResetRangeErrors(OpenableErrors.CannotClose, OpenableErrors.TooLongClosing);
                    return true;
                }

                Open?.Value = false;
                Close?.Value = true;

                var res = true;
                var needWait = false;

                if (IsClose != null && !IsClose.Value)
                    needWait = true;

                if (IsOpen != null && IsOpen.Value)
                    needWait = true;


                if (needWait)
                {
                    token.ThrowIfCancellationRequested();
                    if (IsClose != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                            IsClose,
                            (bool x) =>
                            {
                                if (IsClose != null)
                                    return IsClose.Value;

                                if (IsOpen != null)
                                    return !IsOpen.Value;

                                return true;
                            },
                            TimeForWarning.Value * 1000, token);
                    }
                    else if (IsOpen != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                            IsOpen,
                            (bool x) => !IsOpen.Value,
                            TimeForWarning.Value * 1000, token);
                    }
                }
                if (!res)
                {
                    Logger.LogWarning($"{DeviceName}: долгое закрытие, предупреждение");
                    DeviceErrors.AddError(OpenableErrors.TooLongClosing);
                    if (IsClose != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                            IsClose,
                            (bool x) =>
                            {
                                if (IsClose != null)
                                    return IsClose.Value;

                                if (IsOpen != null)
                                    return !IsOpen.Value;

                                return true;
                            },
                            TimeForError.Value * 1000, token);
                    }
                    else if (IsOpen != null)
                    {
                        res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                            IsOpen,
                            (bool x) => !IsOpen.Value,
                            TimeForError.Value * 1000, token);
                    }
                    if (!res)
                    {
                        Logger.LogError($"{DeviceName}: не смог закрыться, авария");
                        DeviceErrors.AddError(OpenableErrors.CannotClose);
                        Close?.Value = false;
                        return false;
                    }
                }
                DeviceErrors.ResetRangeErrors(OpenableErrors.CannotClose, OpenableErrors.TooLongClosing);
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
    }
}
