using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Throttle
{
    public class Throttle : MechanicDevice<ThrottlePosition, ThrottleErrors>
    {
        public Throttle(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {
        }


        public async Task<bool> Open()
        {
            if (MapState(State) == ThrottlePosition.Throttling)
                return await Move(ThrottlePosition.Throttling, ThrottlePosition.Open) == ThrottlePosition.Open;
            else if (MapState(State) == ThrottlePosition.Close)
                return await Move(ThrottlePosition.Close, ThrottlePosition.Open) == ThrottlePosition.Open;
            else return false;
        }


        public async Task<bool> Close()
        {
            if (MapState(State) == ThrottlePosition.Throttling)
                return await Move(ThrottlePosition.Throttling, ThrottlePosition.Close) == ThrottlePosition.Close;
            else if (MapState(State) == ThrottlePosition.Open)
                return await Move(ThrottlePosition.Open, ThrottlePosition.Close) == ThrottlePosition.Close;
            else return false;
        }


        public async Task<bool> Throttling()
        {
            if (MapState(State) == ThrottlePosition.Close)
                return await Move(ThrottlePosition.Close, ThrottlePosition.Throttling) == ThrottlePosition.Throttling;
            else if (MapState(State) == ThrottlePosition.Open)
                return await Move(ThrottlePosition.Open, ThrottlePosition.Throttling) == ThrottlePosition.Throttling;
            else return false;
        }

        public override MechanicMovingProfile<ThrottleErrors> GetMovingProfile(ThrottlePosition startPos, ThrottlePosition endPos)
        {
            if (startPos == endPos)
                throw new Exception("позиции перемещения стола совпадают");
            var startPosError = ThrottleErrors.IndefinitePosition;
            var revers = endPos - startPos < 0;
            var tormos = true;
            InputSignal<bool> startPosSignal = GetThrottleInputSignalFromPos(startPos);
            InputSignal<bool> endPosSignal = GetThrottleInputSignalFromPos(endPos);
            OutputSignal<bool> endPosOutSignal = GetThrottleOutputSignalFromPos(endPos);
            ThrottleErrors endPosError = GetEndPosError(startPos, endPos);
            return new MechanicMovingProfile<ThrottleErrors>(endPosOutSignal, startPosSignal, endPosSignal, revers, tormos, endPosError, startPosError);
        }

        private InputSignal<bool> GetThrottleInputSignalFromPos(ThrottlePosition pos)
        {
            if (pos == ThrottlePosition.Throttling)
                return Position3In;
            if (pos == ThrottlePosition.Open)
                return Position2In;
            if (pos == ThrottlePosition.Close)
                return Position1In;
            throw new Exception("неверная позиция дроссельного затвора");
        }

        private OutputSignal<bool> GetThrottleOutputSignalFromPos(ThrottlePosition pos)
        {
            if (pos == ThrottlePosition.Throttling)
                return Position3Out;
            if (pos == ThrottlePosition.Open)
                return Position2Out;
            if (pos == ThrottlePosition.Close)
                return Position1Out;
            throw new Exception("неверная позиция дроссельного затвора");
        }

        private static ThrottleErrors GetEndPosError(ThrottlePosition startPos, ThrottlePosition endPos)
        {
            if (endPos == ThrottlePosition.Close)
                return ThrottleErrors.CannotClose;
            if (endPos == ThrottlePosition.Open)
                return ThrottleErrors.CannotOpen;
            if (endPos == ThrottlePosition.Throttling)
                return ThrottleErrors.CannotThrottling;
            throw new Exception("Неверные начальная или конечная позиция");
        }

        protected override ThrottlePosition MapState(MechanicsPositions position) => position switch
        {
            MechanicsPositions.Position1 => ThrottlePosition.Close,
            MechanicsPositions.Position2 => ThrottlePosition.Open,
            MechanicsPositions.Position3 => ThrottlePosition.Throttling,
            MechanicsPositions.Indefinite => ThrottlePosition.Indefinite,
            MechanicsPositions.Uncertain => ThrottlePosition.Uncertain,
            MechanicsPositions.Transition => ThrottlePosition.Transition,
            _ => ThrottlePosition.Uncertain
        };

        protected override ThrottleErrors MapError(MechanicsErrors error) => error switch
        {
            MechanicsErrors.NotInited => ThrottleErrors.NotInited,
            MechanicsErrors.NotInStartPos => ThrottleErrors.IndefinitePosition,
            MechanicsErrors.IndefinitePos => ThrottleErrors.IndefinitePosition,
            MechanicsErrors.UnsertainPos => ThrottleErrors.UncertainPosition,
            MechanicsErrors.NotComeInPos1 => ThrottleErrors.CannotClose,
            MechanicsErrors.NotComeInPos2 => ThrottleErrors.CannotOpen,
            MechanicsErrors.NotComeInPos3 => ThrottleErrors.CannotThrottling,
            MechanicsErrors.None => ThrottleErrors.None,
            _ => ThrottleErrors.CannotClose
        };

        protected override MechanicsErrors ToBaseError(ThrottleErrors error) => error switch
        {
            ThrottleErrors.NotInited => MechanicsErrors.NotInited,
            ThrottleErrors.IndefinitePosition => MechanicsErrors.IndefinitePos,
            ThrottleErrors.UncertainPosition => MechanicsErrors.UnsertainPos,
            ThrottleErrors.CannotClose => MechanicsErrors.NotComeInPos1,
            ThrottleErrors.CannotOpen => MechanicsErrors.NotComeInPos2,
            ThrottleErrors.CannotThrottling => MechanicsErrors.NotComeInPos3,
            ThrottleErrors.None => MechanicsErrors.None,
            _ => MechanicsErrors.NotInEndPos
        };
    }
}
