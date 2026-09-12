using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Carriage
{
    public class Carriage : MechanicDevice<CarriagePosition, CarriageErrors>
    {
        public Carriage(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
            : base(deviceId, signals, loggerFactory, settings)
        {
        }

        [DeviceAction("Отправить каретку")]
        public async Task<bool> MoveCarriage([DeviceActionParameter("позиция")] int position)
        {
            try
            {
                var state = MapState(State);
                return await Move(state, (CarriagePosition)position) == (CarriagePosition)position;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public override MechanicMovingProfile<CarriageErrors> GetMovingProfile(CarriagePosition startPos, CarriagePosition endPos)
        {
            if (startPos == endPos)
                throw new Exception("позиции перемещения каретки совпадают");
            var startPosError = CarriageErrors.NotInStartPosition;
            var revers = endPos - startPos < 0;
            var tormos = true;
            var startPosSignal = GetCarriageInputSignalFromPos(startPos);
            var endPosSignal = GetCarriageInputSignalFromPos(endPos);
            var endPosOutSignal = GetCarriageOutputSignalFromPos(endPos);
            var endPosError = GetEndPosError(startPos, endPos);
            return new MechanicMovingProfile<CarriageErrors>(endPosOutSignal, startPosSignal, endPosSignal, revers, tormos, endPosError, startPosError);
        }

        private InputSignal<bool> GetCarriageInputSignalFromPos(CarriagePosition pos)
        {
            if (pos == CarriagePosition.Position1)
                return Position1In;
            if (pos == CarriagePosition.Position2)
                return Position2In;
            if (pos == CarriagePosition.Position3)
                return Position3In;
            if (pos == CarriagePosition.Position4)
                return Position4In;
            if (pos == CarriagePosition.Position5)
                return Position5In;
            if (pos == CarriagePosition.Position6)
                return Position6In;
            throw new InvalidOperationException("неверная позиция каретки");
        }

        private OutputSignal<bool> GetCarriageOutputSignalFromPos(CarriagePosition pos)
        {
            if (pos == CarriagePosition.Position1)
                return Position1Out;
            if (pos == CarriagePosition.Position2)
                return Position2Out;
            if (pos == CarriagePosition.Position3)
                return Position3Out;
            if (pos == CarriagePosition.Position4)
                return Position4Out;
            if (pos == CarriagePosition.Position5)
                return Position5Out;
            if (pos == CarriagePosition.Position6)
                return Position6Out;
            throw new InvalidOperationException("неверная позиция каретки");
        }

        protected override CarriageErrors MapError(MechanicsErrors errors) => errors switch
        {
            MechanicsErrors.NotInited => CarriageErrors.NotInited,
            MechanicsErrors.NotInStartPos => CarriageErrors.NotInStartPosition,
            MechanicsErrors.IndefinitePos => CarriageErrors.IndefinitePosition,
            MechanicsErrors.UnsertainPos => CarriageErrors.UncertainPosition,
            MechanicsErrors.None => CarriageErrors.None,
            _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {errors} в CarriageErrors")
        };

        protected override CarriagePosition MapState(MechanicsPositions position) => position switch
        {

            MechanicsPositions.Position1 => CarriagePosition.Position1,
            MechanicsPositions.Position2 => CarriagePosition.Position2,
            MechanicsPositions.Position3 => CarriagePosition.Position3,
            MechanicsPositions.Position4 => CarriagePosition.Position4,
            MechanicsPositions.Position5 => CarriagePosition.Position5,
            MechanicsPositions.Position6 => CarriagePosition.Position6,
            MechanicsPositions.Indefinite => CarriagePosition.Indefinite,
            MechanicsPositions.Uncertain => CarriagePosition.Uncertain,
            MechanicsPositions.Transition => CarriagePosition.Transition,
            _ => CarriagePosition.Uncertain
        };

        protected override MechanicsErrors ToBaseError(CarriageErrors error) => error switch
        {
            CarriageErrors.NotInited => MechanicsErrors.NotInited,
            CarriageErrors.IndefinitePosition => MechanicsErrors.IndefinitePos,
            CarriageErrors.NotInStartPosition => MechanicsErrors.NotInStartPos,
            CarriageErrors.UncertainPosition => MechanicsErrors.UnsertainPos,
            CarriageErrors.NotCameInPosition1 => MechanicsErrors.NotComeInPos1,
            CarriageErrors.NotCameInPosition2 => MechanicsErrors.NotComeInPos2,
            CarriageErrors.NotCameInPosition3 => MechanicsErrors.NotComeInPos3,
            CarriageErrors.NotCameInPosition4 => MechanicsErrors.NotComeInPos4,
            CarriageErrors.NotCameInPosition5 => MechanicsErrors.NotComeInPos5,
            CarriageErrors.NotCameInPosition6 => MechanicsErrors.NotComeInPos6,
            CarriageErrors.None => MechanicsErrors.None,
            _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {error} в MechanicsErrors")
        };

        private static CarriageErrors GetEndPosError(CarriagePosition startPos, CarriagePosition endPos)
        {
            if (endPos == CarriagePosition.Position1)
                return CarriageErrors.NotCameInPosition1;
            if (endPos == CarriagePosition.Position2)
                return CarriageErrors.NotCameInPosition2;
            if (endPos == CarriagePosition.Position3)
                return CarriageErrors.NotCameInPosition3;
            if (endPos == CarriagePosition.Position4)
                return CarriageErrors.NotCameInPosition4;
            if (endPos == CarriagePosition.Position5)
                return CarriageErrors.NotCameInPosition5;
            if (endPos == CarriagePosition.Position6)
                return CarriageErrors.NotCameInPosition6;
            throw new InvalidOperationException("Неверные начальная или конечная позиция");
        }
    }
}
