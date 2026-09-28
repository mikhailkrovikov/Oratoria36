using Microsoft.Extensions.Logging;
using NLog;
using Oratoria.Domain.Devices.Abstractions.MechanicAttributes;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;
using System.Reflection;

namespace Oratoria.Domain.Devices.Abstractions
{
    public abstract class MechanicDevice<TPos, TErr> :
        Device<MechanicsPositions, MechanicsErrors>
        where TPos : Enum
        where TErr : Enum
    {
        private const int DELAY = 1000;
        private readonly Dictionary<MechanicsPositions, TPos> _positionMap = new();
        private readonly Dictionary<MechanicsErrors, TErr> _errorMap = new();
        private readonly Dictionary<TErr, MechanicsErrors> _baseErrorMap = new();

        protected abstract MechanicMovingProfile<TErr> GetMovingProfile(TPos startPos, TPos endPos);

        protected TPos MapState(MechanicsPositions position)
        {
            if (_positionMap.TryGetValue(position, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"Не удалось преобразовать {position} в {typeof(TPos).Name}.");
            }
        }

        public TErr MapError(MechanicsErrors error)
        {
            if (_errorMap.TryGetValue(error, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"Не удалось преобразовать {error} в {typeof(TErr).Name}.");
            }
        }

        protected MechanicsErrors ToBaseError(TErr error)
        {
            if (_baseErrorMap.TryGetValue(error, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"Не удалось преобразовать {typeof(TErr).Name}.{error} в MechanicsErrors.");
            }
        }

        public InputSignal<bool> Position1In { get; set; }

        public InputSignal<bool> Position2In { get; set; }

        public InputSignal<bool> Position3In { get; set; }

        public InputSignal<bool> Position4In { get; set; }

        public InputSignal<bool> Position5In { get; set; }

        public InputSignal<bool> Position6In { get; set; }

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

        public OutputSignal<bool> Position5Out { get; set; }

        public OutputSignal<bool>? Position6Out { get; set; }


        private MechanicsPositions _state = MechanicsPositions.Indefinite;

        public override MechanicsPositions State => _state;

        public TPos Position => MapState(State);

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

        public MechanicDevice(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(deviceId, signals, loggerFactory, settings)
        {
            Position1In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition1InputSignalAttribute<>))!;
            Position2In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition2InputSignalAttribute<>))!;
            Position3In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition3InputSignalAttribute<>))!;
            Position4In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition4InputSignalAttribute<>));
            Position5In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition5InputSignalAttribute<>));
            Position6In = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicPosition6InputSignalAttribute<>));
            TormosIn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicTormosInputSignalAttribute<>))!;
            ReversIn = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicReversInputSignalAttribute<>))!;
            DriverOverload = SignalHelper<InputSignal<bool>>.GetSignal(deviceId, signals.DISignals, typeof(MechanicDriverOverloadInputSignalAttribute<>))!;

            Position1Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition1OutputSignalAttribute<>))!;
            Position2Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition2OutputSignalAttribute<>))!;
            Position3Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition3OutputSignalAttribute<>))!;
            Position4Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition4OutputSignalAttribute<>))!;
            Position5Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition5OutputSignalAttribute<>))!;
            Position6Out = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicPosition6OutputSignalAttribute<>))!;
            TormosOut = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicTormosOutputSignalAttribute<>))!;
            ReversOut = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicReversOutputSignalAttribute<>))!;
            Actuator = SignalHelper<OutputSignal<bool>>.GetSignal(deviceId, signals.DOSignals, typeof(MechanicDriverOutputSignalAttribute<>))!;

            DriverOverload.OnSignalChanged += DriverOverloadHandler;
            Position1In.OnSignalChanged += _ => OnPositionChanged();
            Position2In.OnSignalChanged += _ => OnPositionChanged();
            Position3In.OnSignalChanged += _ => OnPositionChanged();

            ActionTime = Settings.GetSetting(deviceId, nameof(ActionTime), "Время движения актуатора", "сек", 30);

            foreach (var field in typeof(TPos).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<MechanicStatusMappingAttribute>();
                var position = (TPos)field.GetValue(null)!;
                _positionMap.Add(attribute.Position, position);
            }

            foreach (var field in typeof(TErr).GetFields(
                BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<MechanicErrorMappingAttribute>();
                var error = (TErr)field.GetValue(null)!;
                _baseErrorMap.Add(error, attribute.Error);
                _errorMap.Add(attribute.Error, error);
            }
        }

        protected void DriverOverloadHandler(bool value)
        {
            if (value)
                Logger.LogWarning($"{DeviceName}: перегруз привода");
        }


        [DeviceAction("Инициализация")]
        public Task<TPos> Init(CancellationToken cancellationToken = default)
        {
            return RunOperation(cancellationToken, async token =>
            {
                Logger.LogInformation($"{DeviceName}: инициализация");
                Actuator.Value = true;
                try
                {
                    await Task.Delay(DELAY, token);
                    return GetPosition();
                }
                finally
                {
                    Actuator.Value = false;
                }
            });
        }

        private TPos GetPosition()
        {
            var pos1 = Position1In.Value;
            var pos2 = Position2In.Value;
            var pos3 = Position3In.Value;
            var pos4 = Position4In?.Value;
            var pos5 = Position5In?.Value;
            var pos6 = Position6In?.Value;

            var trueCount =
                (pos1 ? 1 : 0) +
                (pos2 ? 1 : 0) +
                (pos3 ? 1 : 0) +
                (pos4 ?? false ? 1 : 0) +
                (pos5 ?? false ? 1 : 0) +
                (pos6 ?? false ? 1 : 0);

            if (trueCount == 0)
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
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position1);
                return MapState(MechanicsPositions.Position1);
            }

            else if (pos2)
            {
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position2);
                return MapState(MechanicsPositions.Position2);
            }

            else if (pos3)
            {
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position3);
                return MapState(MechanicsPositions.Position3);
            }
            else if (pos4 ?? false)
            {
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position4);
                return MapState(MechanicsPositions.Position4);
            }

            else if (pos5 ?? false)
            {
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position5);
                return MapState(MechanicsPositions.Position5);
            }

            else if (pos6 ?? false)
            {
                DeviceErrors.ResetRangeErrors(MechanicsErrors.UnsertainPos, MechanicsErrors.IndefinitePos);
                SetState(MechanicsPositions.Position6);
                return MapState(MechanicsPositions.Position6);
            }

            Logger.LogError($"{DeviceName}: неоднозначное положение");
            DeviceErrors.AddError(MechanicsErrors.UnsertainPos);
            return MapState(MechanicsPositions.Uncertain);
        }

        [DeviceAction("Стоп приводов")]
        public void EmergencyStop()
        {
            Logger.LogInformation($"{DeviceName}: стоп");
            try
            {
                CTSource.Cancel();
            }
            catch { }
            Actuator.Value = false;
            TormosOut.Value = false;
            ReversOut.Value = false;
            Position1Out?.Value = false;
            Position2Out?.Value = false;
            Position3Out?.Value = false;
            Position4Out?.Value = false;
            Position5Out?.Value = false;
            Position6Out?.Value = false;
            ResetToken();
        }

        public Task<TPos> Move(TPos startPos, TPos endPos, CancellationToken cancellationToken = default)
        {
            return RunOperation(cancellationToken, async token =>
            {
                try
                {
                    var movingProfile = GetMovingProfile(startPos, endPos);
                    Actuator.Value = true;
                    await Task.Delay(DELAY, token);
                    if (!movingProfile.StartPosSignal.Value)
                    {
                        Logger.LogError($"{DeviceName}: неверное исходное положение");
                        DeviceErrors.AddError(ToBaseError(movingProfile.StartPosError));
                        Actuator.Value = false;
                        GetPosition();
                        return MapState(State);
                    }

                    if (!await ReversCommand(movingProfile.Revers, token))
                        return MapState(State);

                    if (!await TormosCommand(movingProfile.Tormos, token))
                        return MapState(State);

                    movingProfile.EndPosOutSignal.Value = true;
                    SetState(MechanicsPositions.Transition);

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
                    throw;
                }
            });
        }

        private async Task<bool> TormosCommand(bool value, CancellationToken token)
        {
            TormosOut.Value = value;
            if (TormosIn.Value == value)
                return true;
            var ret = await EventWaiter.WaitEvent(nameof(TormosIn.OnSignalChanged),
                TormosIn,
                (bool v) => TormosIn.Value == value,
                ActionTime.Value * 1000, token);
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
                ActionTime.Value * 1000, token);
            if (!ret)
            {
                Logger.LogWarning($"{DeviceName}: реверс: ошибка обратной связи");
                return false;
            }
            return true;
        }
    }
}
