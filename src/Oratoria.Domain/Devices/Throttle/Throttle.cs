using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Throttle
{
    public class Throttle : MechanicDevice<ThrottlePosition, ThrottleErrors>
    {
        public Throttle(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) 
            : base(deviceId, signals, loggerFactory, settings)
        {
        }

        [DeviceAction("Открыть")]
        public async Task<bool> Open()
        {
            try
            {
                if (MapState(State) == ThrottlePosition.Throttling)
                    return await Move(ThrottlePosition.Throttling, ThrottlePosition.Open) == ThrottlePosition.Open;
                else if (MapState(State) == ThrottlePosition.Close)
                    return await Move(ThrottlePosition.Close, ThrottlePosition.Open) == ThrottlePosition.Open;
                else return false;

            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Закрыть")]
        public async Task<bool> Close()
        {
            try
            {
                if (MapState(State) == ThrottlePosition.Throttling)
                    return await Move(ThrottlePosition.Throttling, ThrottlePosition.Close) == ThrottlePosition.Close;
                else if (MapState(State) == ThrottlePosition.Open)
                    return await Move(ThrottlePosition.Open, ThrottlePosition.Close) == ThrottlePosition.Close;
                else return false;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Дросселирование")]
        public async Task<bool> Throttling()
        {
            try
            {
                if (MapState(State) == ThrottlePosition.Close)
                    return await Move(ThrottlePosition.Close, ThrottlePosition.Throttling) == ThrottlePosition.Throttling;
                else if (MapState(State) == ThrottlePosition.Open)
                    return await Move(ThrottlePosition.Open, ThrottlePosition.Throttling) == ThrottlePosition.Throttling;
                else return false;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public override MechanicMovingProfile<ThrottleErrors> GetMovingProfile(ThrottlePosition startPos, ThrottlePosition endPos)
        {
            if (startPos == endPos)
                throw new InvalidOperationException("позиции перемещения дроссельного затвора совпадают");
            var startPosError = ThrottleErrors.NotInStartPos;
            var revers = endPos - startPos < 0;
            var tormos = true;
            var startPosSignal = GetThrottleInputSignalFromPos(startPos);
            var endPosSignal = GetThrottleInputSignalFromPos(endPos);
            var endPosOutSignal = GetThrottleOutputSignalFromPos(endPos);
            var endPosError = GetEndPosError(endPos);
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
            throw new InvalidOperationException("неверная позиция дроссельного затвора");
        }

        private OutputSignal<bool> GetThrottleOutputSignalFromPos(ThrottlePosition pos)
        {
            if (pos == ThrottlePosition.Throttling)
                return Position3Out;
            if (pos == ThrottlePosition.Open)
                return Position2Out;
            if (pos == ThrottlePosition.Close)
                return Position1Out;
            throw new InvalidOperationException("неверная позиция дроссельного затвора");
        }

        private static ThrottleErrors GetEndPosError(ThrottlePosition endPos)
        {
            if (endPos == ThrottlePosition.Close)
                return ThrottleErrors.CannotClose;
            if (endPos == ThrottlePosition.Open)
                return ThrottleErrors.CannotOpen;
            if (endPos == ThrottlePosition.Throttling)
                return ThrottleErrors.CannotThrottling;
            throw new InvalidOperationException("Неверная конечная позиция");
        }
    }
}
