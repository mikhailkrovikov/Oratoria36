using Microsoft.Extensions.Logging;
using NLog;
using Oratoria.Domain.Devices.Abstractions.MechanicAttributes;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class MechanicDevice<TPos, TErr> :
        Device<MechanicsPositions, MechanicsErrors>
        where TPos : Enum
        where TErr : Enum
    {
        public const int TIME_FOR_WAITING_POS_MILLISEC = 1000;
        public abstract MechanicMovingProfile<TErr> GetMovingProfile(TPos startPos, TPos endPos);

        protected abstract TPos MapState(MechanicsPositions position);

        protected abstract TErr MapError(MechanicsErrors errors);

        protected abstract MechanicsErrors ToBaseError(TErr error);

        public InputSignal<bool> Position1In { get; set; }

        public InputSignal<bool> Position2In { get; set; }

        public InputSignal<bool> Position3In { get; set; }

        public InputSignal<bool> TormosIn { get; set; }

        public InputSignal<bool> ReversIn { get; set; }

        public InputSignal<bool> DriverOverload { get; set; }

        public OutputSignal<bool> Actuator { get; set; }

        public OutputSignal<bool> TormosOut { get; set; }

        public OutputSignal<bool> ReversOut { get; set; }

        public OutputSignal<bool> Position1Out { get; set; }

        public OutputSignal<bool> Position2Out { get; set; }

        public OutputSignal<bool> Position3Out { get; set; }

        public OutputSignal<bool>? Position4Out { get; set; }


        private MechanicsPositions _state = MechanicsPositions.Indefinite;

        public override MechanicsPositions State => _state;

        private void SetState(MechanicsPositions value)
        {
            if (_state.Equals(value)) return;
            _state = value;
            OnStateChanged();
        }


        public event Action? PositionChanged;
        protected void OnPositionChanged()
        {
            PositionChanged?.Invoke();
        }


        public Setting<int> ActionTime;

        public MechanicDevice(Enum deviceId,IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, loggerFactory)
        {
            Position1In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition1InputSignalAttribute<>))!;
            Position2In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition2InputSignalAttribute<>))!;
            Position3In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition3InputSignalAttribute<>))!;
            TormosIn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicTormosInputSignalAttribute<>))!;
            ReversIn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicReversInputSignalAttribute<>))!;
            DriverOverload = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicDriverOverloadInputSignalAttribute<>))!;
            Position1Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition1OutputSignalAttribute<>))!;
            Position2Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition2OutputSignalAttribute<>))!;
            Position3Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition3OutputSignalAttribute<>))!;
            TormosOut = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicTormosOutputSignalAttribute<>))!;
            ReversOut = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicReversOutputSignalAttribute<>))!;

            DriverOverload.OnSignalChanged += DriverOverloadHandler;
            Position1In.OnSignalChanged += _ => OnPositionChanged();
            Position2In.OnSignalChanged += _ => OnPositionChanged();
            Position3In.OnSignalChanged += _ => OnPositionChanged();

            ActionTime = new(DeviceId, "Время движения актуатора", "сек");       
        }

        protected void DriverOverloadHandler(bool value)
        {
            if (value)
                Logger.LogWarning($"{DeviceName}: перегруз привода");
        }

        public void EmergencyStop()
        {
            try
            {
                CTSource.Cancel();
            }
            catch { }
            Actuator.Value = false;
            TormosOut.Value = false;
            ReversOut.Value = false;
            Position1Out.Value = false;
            Position2Out.Value = false;
            Position3Out.Value = false;
            Logger.LogDebug($"{DeviceName}: стоп");
            ResetToken();
        }

        public async Task<TPos> Init()
        {
            Logger.LogInformation($"{DeviceName}: инициализация");
            Actuator.Value = true;
            await Task.Delay(1000);
            var result = GetPosition();
            Actuator.Value = false;
            return result;

        }

        private TPos GetPosition()
        {
            var pos1 = Position1In.Value;
            var pos2 = Position2In.Value;
            var pos3 = Position3In.Value;

            var trueCount = (pos1 ? 1 : 0) + (pos2 ? 1 : 0) + (pos3 ? 1 : 0);

            if (!pos1 && !pos2 && !pos3)
            {
                Logger.LogError($"{DeviceName}: неопределенное положение");
                DeviceErrors.AddError(MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Indefinite);
                return MapState(MechanicsPositions.Indefinite);
            }

            else if (trueCount > 1)
            {
                Logger.LogError($"{DeviceName}: неоднозначное положение");
                DeviceErrors.AddError(MechanicsErrors.UnsertainPos);
                SetState(MechanicsPositions.Uncertain);
                return MapState(MechanicsPositions.Uncertain);
            }

            else if (pos1)
            {
                DeviceErrors.ResetAllErrors();
                SetState(MechanicsPositions.Position1);
                return MapState(MechanicsPositions.Position1);
            }

            else if (pos2)
            {
                DeviceErrors.ResetAllErrors();
                SetState(MechanicsPositions.Position2);
                return MapState(MechanicsPositions.Position2);
            }

            else if (pos3)
            {
                DeviceErrors.ResetAllErrors();
                SetState(MechanicsPositions.Position3);
                return MapState(MechanicsPositions.Position3);
            }

            Logger.LogError($"{DeviceName}: неоднозначное положение");
            DeviceErrors.AddError(MechanicsErrors.UnsertainPos);
            return MapState(MechanicsPositions.Uncertain);
        }

        public void AlarmStop()
        {
            try
            {
                CTSource.Cancel();
            }
            catch { }
            Actuator.Value = false;
            Position1Out.Value = false;
            Position2Out.Value = false;
            Position3Out.Value = false;
            ReversOut.Value = false;
            TormosOut.Value = false;
            ResetToken();
        }

        public void ResetErrors()
        {
            Logger.LogInformation($"{DeviceName}: сброс ошибок");
            DeviceErrors.ResetAllErrors();
        }


        private Task<bool> GetMovingTask(InputSignal<bool> targetPosition)
        {
            return EventWaiter.WaitEvent(nameof(targetPosition.OnSignalChanged),
                    targetPosition,
                    (bool value) => targetPosition.Value == true,
                    ActionTime.Value * 1000,
                    CTSource.Token);
        }

        public async Task<TPos> Move(TPos startPos, TPos endPos)
        {
            ResetToken();
            var token = CTSource.Token;
            var movingProfile = GetMovingProfile(startPos, endPos);

            try
            {
                Actuator.Value = true;
                await Task.Delay(TIME_FOR_WAITING_POS_MILLISEC, token);
                if (!movingProfile.StartPosSignal.Value)
                {
                    Logger.LogError($"{DeviceName}: неверное исходное положение");
                    DeviceErrors.AddError(ToBaseError(movingProfile.StartPosError));
                    Actuator.Value = false;
                    GetPosition();
                    return MapState(State);
                }

                await ReversCommand(movingProfile.Revers, token);
                await TormosCommand(movingProfile.Tormos, token);

                movingProfile.EndPosOutSignal.Value = true;
                SetState(MechanicsPositions.Transition);

                var positionReached = await GetMovingTask(movingProfile.EndPosSignal);
                token.ThrowIfCancellationRequested();
                if (!positionReached)
                {
                    Logger.LogWarning($"{DeviceName}: превышено время движения");
                    positionReached = await GetMovingTask(movingProfile.EndPosSignal);
                }

                movingProfile.EndPosOutSignal.Value = false;
                await ReversCommand(false, token);
                await TormosCommand(false, token);


                if (!movingProfile.EndPosSignal.Value)
                {
                    Logger.LogError($"{DeviceName}: неверное конечное положение");
                    DeviceErrors.AddError(ToBaseError(movingProfile.EndPosError));
                    Actuator.Value = false;
                    GetPosition();
                    return MapState(State);
                }
                Actuator.Value = false;
                GetPosition();
                DeviceErrors.ResetRangeErrors(
                    ToBaseError(movingProfile.StartPosError),
                    ToBaseError(movingProfile.EndPosError));

                return MapState(State);
            }
            catch (OperationCanceledException)
            {
                Logger.LogWarning($"{DeviceName}: движение прервано");
                Actuator.Value = false;
                GetPosition();
                return MapState(State);
            }
        }

        private async Task<bool> TormosCommand(bool value, CancellationToken token)
        {
            TormosOut.Value = value;
            if (TormosIn.Value == value)
                return true;
            var ret = await EventWaiter.WaitEvent(nameof(TormosIn.OnSignalChanged),
                TormosIn,
                (bool v) => TormosIn.Value == value,
                TIME_FOR_WAITING_POS_MILLISEC, token);
            if (!ret)
            {
                Logger.LogWarning($"{DeviceName}: тормоз: ошибка обратной связи");
                return false;
            }
            return true;
        }

        private async Task<bool> ReversCommand(bool value, CancellationToken token)
        {
            ReversOut.Value = value;
            if (ReversIn.Value == value)
                return true;
            var ret = await EventWaiter.WaitEvent(nameof(ReversIn.OnSignalChanged),
                ReversIn,
                (bool v) => ReversIn.Value == value,
                TIME_FOR_WAITING_POS_MILLISEC, token);
            if (!ret)
            {
                Logger.LogWarning($"{DeviceName}: реверс: ошибка обратной связи");
                return false;
            }
            return true;
        }
    }

    public class MechanicMovingProfile<TErr>(
        OutputSignal<bool> outPos, 
        InputSignal<bool> startPos,
        InputSignal<bool> endPos, 
        bool revers, 
        bool tormos, 
        TErr endPosError, 
        TErr startPosError) where TErr : Enum
    {
        public OutputSignal<bool> EndPosOutSignal { get; } = outPos;

        public InputSignal<bool> StartPosSignal { get; } = startPos;

        public InputSignal<bool> EndPosSignal { get; } = endPos;

        public bool Revers { get; } = revers;

        public bool Tormos { get; } = tormos;

        public TErr EndPosError { get; } = endPosError;

        public TErr StartPosError { get; } = startPosError;
    }
}
