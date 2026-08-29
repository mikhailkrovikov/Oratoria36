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
                if (IsClose?.Value == IsOpen?.Value)
                    return OpenableStatus.Transition;
                else if (IsClose.Value)
                    return OpenableStatus.Close;
                else if (IsOpen.Value)
                    return OpenableStatus.Open;
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

                bool res = true;
                if (!(IsOpen.Value && !IsClose.Value))
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                        IsOpen,
                        (bool x) => IsOpen.Value && !IsClose.Value,
                        TimeForWarning.Value * 1000, token);
                }

                if (!res)
                {
                    Logger.LogWarning($"{DeviceName}: долгое открытие, предупреждение");
                    DeviceErrors.AddError(OpenableErrors.TooLongOpening);
                    res = await EventWaiter.WaitEvent(nameof(IsOpen.OnSignalChanged),
                        IsOpen,
                        (bool x) => IsOpen.Value && !IsClose.Value,
                        TimeForError.Value * 1000, token);

                    if (!res)
                    {
                        Logger.LogError($"{DeviceName}: не смог открыться, авария");
                        DeviceErrors.AddError(OpenableErrors.CannotOpen);
                        Open.Value = false;
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

                bool res = true;
                if (!(IsClose.Value && !IsOpen.Value))
                {
                    token.ThrowIfCancellationRequested();
                    res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                        IsClose,
                        (bool x) => IsClose.Value && !IsOpen.Value,
                        TimeForWarning.Value * 1000, token);
                }

                if (!res)
                {
                    Logger.LogWarning($"{DeviceName}: долгое закрытие, предупреждение");
                    DeviceErrors.AddError(OpenableErrors.TooLongClosing);
                    res = await EventWaiter.WaitEvent(nameof(IsClose.OnSignalChanged),
                        IsClose,
                        (bool x) => IsClose.Value && !IsOpen.Value,
                        TimeForError.Value * 1000, token);

                    if (!res)
                    {
                        Logger.LogError($"{DeviceName}: не смог закрыться, авария");
                        DeviceErrors.AddError(OpenableErrors.CannotClose);
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
