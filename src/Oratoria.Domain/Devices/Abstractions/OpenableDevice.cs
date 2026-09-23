using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class OpenableDevice : Device<OpenableStatus, OpenableErrors>
    {
        public InputSignal<bool>? IsOpenSignal { get; set; }

        public InputSignal<bool>? IsCloseSignal { get; set; }

        public OutputSignal<bool>? OpenSignal { get; set; }

        public OutputSignal<bool>? CloseSignal { get; set; }

        public Setting<int> TimeForWarning { get; }

        public Setting<int> TimeForError { get; }

        public override OpenableStatus State
        {
            get
            {
                if (IsOpenSignal != null && IsCloseSignal != null)
                {
                    if (IsCloseSignal.Value == IsOpenSignal.Value)
                        return OpenableStatus.Transition;
                    if (IsCloseSignal.Value)
                        return OpenableStatus.Close;
                    if (IsOpenSignal.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Transition;
                }
                if (IsOpenSignal != null)
                {
                    if (IsOpenSignal.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Close;
                }
                if (IsCloseSignal != null)
                {
                    if (IsCloseSignal.Value)
                        return OpenableStatus.Close;
                    return OpenableStatus.Open;
                }
                if (OpenSignal != null)
                {
                    if (OpenSignal.Value)
                        return OpenableStatus.Open;
                    return OpenableStatus.Close;
                }
                if (CloseSignal != null)
                {
                    if (CloseSignal.Value)
                        return OpenableStatus.Close;
                    return OpenableStatus.Open;
                }
                return OpenableStatus.Transition;
            }
        }

        protected OpenableDevice(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            TimeForWarning = Settings.GetSetting(deviceId, nameof(TimeForWarning), "Время до предупреждения", "сек", 5, 1, 3600);
            TimeForError = Settings.GetSetting(deviceId, nameof(TimeForError), "Время до ошибки", "сек", 15, 1, 3600);
        }

        [DeviceAction("Открыть")]
        public virtual Task<bool> OpenValve(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"{DeviceName}: открытие");
            return RunOperation(cancellationToken, async token =>
            {
                try
                {
                    if (State == OpenableStatus.Open)
                    {
                        DeviceErrors.ResetRangeErrors(OpenableErrors.CannotOpen, OpenableErrors.TooLongOpening);
                        return true;
                    }

                    OpenSignal?.Value = true;
                    CloseSignal?.Value = false;

                    var res = true;
                    var needWait = false;

                    if (IsOpenSignal != null && !IsOpenSignal.Value)
                        needWait = true;

                    if (IsCloseSignal != null && IsCloseSignal.Value)
                        needWait = true;


                    if (needWait)
                    {
                        token.ThrowIfCancellationRequested();
                        if (IsOpenSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsOpenSignal.OnSignalChanged),
                                IsOpenSignal,
                                (bool x) =>
                                {
                                    if (IsOpenSignal != null)
                                        return IsOpenSignal.Value;

                                    if (IsCloseSignal != null)
                                        return !IsCloseSignal.Value;

                                    return true;
                                },
                                TimeForWarning.Value * 1000, token);
                        }
                        else if (IsCloseSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsCloseSignal.OnSignalChanged),
                                IsCloseSignal,
                                (bool x) => !IsCloseSignal.Value,
                                TimeForWarning.Value * 1000, token);
                        }
                    }
                    if (!res)
                    {
                        Logger.LogWarning($"{DeviceName}: долгое открытие, предупреждение");
                        DeviceErrors.AddError(OpenableErrors.TooLongOpening);
                        if (IsOpenSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsOpenSignal.OnSignalChanged),
                                IsOpenSignal,
                                (bool x) =>
                                {
                                    if (IsOpenSignal != null)
                                        return IsOpenSignal.Value;

                                    if (IsCloseSignal != null)
                                        return !IsCloseSignal.Value;

                                    return true;
                                },
                                TimeForError.Value * 1000, token);
                        }
                        else if (IsCloseSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsCloseSignal.OnSignalChanged),
                                IsCloseSignal,
                                (bool x) => !IsCloseSignal.Value,
                                TimeForError.Value * 1000, token);
                        }
                        if (!res)
                        {
                            Logger.LogError($"{DeviceName}: не смог открыться, авария");
                            DeviceErrors.AddError(OpenableErrors.CannotOpen);
                            OpenSignal?.Value = false;
                            return false;
                        }
                    }
                    DeviceErrors.ResetRangeErrors(OpenableErrors.CannotOpen, OpenableErrors.TooLongOpening);
                    return true;
                }
                catch (OperationCanceledException)
                {
                    Logger.LogInformation($"{DeviceName}: открытие отменено");
                    OpenSignal?.Value = false;
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"{DeviceName}: ошибка открытия");
                    Logger.LogError(ex.Message);
                    return false;
                }
            });
        }


        [DeviceAction("Закрыть")]
        public virtual Task<bool> CloseValve(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"{DeviceName}: закрытие");
            return RunOperation(cancellationToken, async token =>
            {
                try
                {
                    if (State == OpenableStatus.Close)
                    {
                        DeviceErrors.ResetRangeErrors(OpenableErrors.CannotClose, OpenableErrors.TooLongClosing);
                        return true;
                    }

                    OpenSignal?.Value = false;
                    CloseSignal?.Value = true;

                    var res = true;
                    var needWait = false;

                    if (IsCloseSignal != null && !IsCloseSignal.Value)
                        needWait = true;

                    if (IsOpenSignal != null && IsOpenSignal.Value)
                        needWait = true;


                    if (needWait)
                    {
                        token.ThrowIfCancellationRequested();
                        if (IsCloseSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsCloseSignal.OnSignalChanged),
                                IsCloseSignal,
                                (bool x) =>
                                {
                                    if (IsCloseSignal != null)
                                        return IsCloseSignal.Value;

                                    if (IsOpenSignal != null)
                                        return !IsOpenSignal.Value;

                                    return true;
                                },
                                TimeForWarning.Value * 1000, token);
                        }
                        else if (IsOpenSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsOpenSignal.OnSignalChanged),
                                IsOpenSignal,
                                (bool x) => !IsOpenSignal.Value,
                                TimeForWarning.Value * 1000, token);
                        }
                    }
                    if (!res)
                    {
                        Logger.LogWarning($"{DeviceName}: долгое закрытие, предупреждение");
                        DeviceErrors.AddError(OpenableErrors.TooLongClosing);
                        if (IsCloseSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsCloseSignal.OnSignalChanged),
                                IsCloseSignal,
                                (bool x) =>
                                {
                                    if (IsCloseSignal != null)
                                        return IsCloseSignal.Value;

                                    if (IsOpenSignal != null)
                                        return !IsOpenSignal.Value;

                                    return true;
                                },
                                TimeForError.Value * 1000, token);
                        }
                        else if (IsOpenSignal != null)
                        {
                            res = await EventWaiter.WaitEvent(nameof(IsOpenSignal.OnSignalChanged),
                                IsOpenSignal,
                                (bool x) => !IsOpenSignal.Value,
                                TimeForError.Value * 1000, token);
                        }
                        if (!res)
                        {
                            Logger.LogError($"{DeviceName}: не смог закрыться, авария");
                            DeviceErrors.AddError(OpenableErrors.CannotClose);
                            CloseSignal?.Value = false;
                            return false;
                        }
                    }
                    DeviceErrors.ResetRangeErrors(OpenableErrors.CannotClose, OpenableErrors.TooLongClosing);
                    return true;
                }
                catch (OperationCanceledException)
                {
                    Logger.LogInformation($"{DeviceName}: закрытие отменено");
                    CloseSignal?.Value = false;
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"{DeviceName}: ошибка закрытия");
                    Logger.LogError(ex.Message);
                    return false;
                }
            });
        }
    }
}
