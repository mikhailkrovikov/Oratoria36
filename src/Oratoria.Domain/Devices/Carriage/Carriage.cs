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
            Logger.LogInformation($"в позицию {position}");
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
                throw new InvalidOperationException("позиции перемещения каретки совпадают");
            var startPosError = CarriageErrors.NotInStartPosition;
            var revers = endPos - startPos < 0;
            var tormos = true;
            var startPosSignal = GetCarriageInputSignalFromPos(startPos);
            var endPosSignal = GetCarriageInputSignalFromPos(endPos);
            var endPosOutSignal = GetCarriageOutputSignalFromPos(endPos);
            var endPosError = GetEndPosError(endPos);
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

        private static CarriageErrors GetEndPosError(CarriagePosition endPos)
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
            throw new InvalidOperationException("Неверная конечная позиция");
        }
    }
}
