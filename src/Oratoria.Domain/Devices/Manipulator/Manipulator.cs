using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Manipulator
{
    public class Manipulator : MechanicDevice<ManipulatorPosition, ManipulatorErrors>
    {
        public Manipulator(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
            : base(deviceId, signals, loggerFactory, settings)
        {
        }

        [DeviceAction("Исходная → Транспорт")]
        public async Task<bool> FromHomeToTransport()
        {
            try
            {
                return await Move(ManipulatorPosition.Home, ManipulatorPosition.Transport) == ManipulatorPosition.Transport;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Модуль → Исходная")]
        public async Task<bool> FromModuleToHome()
        {
            try
            {
                return await Move(ManipulatorPosition.Module, ManipulatorPosition.Home) == ManipulatorPosition.Home;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Исходная → Модуль")]
        public async Task<bool> FromHomeToModule()
        {
            try
            {
                return await Move(ManipulatorPosition.Home, ManipulatorPosition.Module) == ManipulatorPosition.Module;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Транспорт → Исходная")]
        public async Task<bool> FromTransportToHome()
        {
            try
            {
                return await Move(ManipulatorPosition.Transport, ManipulatorPosition.Home) == ManipulatorPosition.Home;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public override MechanicMovingProfile<ManipulatorErrors> GetMovingProfile(ManipulatorPosition startPos, ManipulatorPosition endPos)
        {
            if (startPos == endPos)
                throw new InvalidOperationException("позиции перемещения манипулятора совпадают");
            if (startPos != ManipulatorPosition.Home && endPos != ManipulatorPosition.Home)
                throw new InvalidOperationException("неверные позиции перемещения манипулятора");
            var startPosError = ManipulatorErrors.NotInStartPos;
            var revers = endPos - startPos < 0;
            var tormos = true;
            var startPosSignal = GetManInputSignalFromPos(startPos);
            var endPosSignal = GetManInputSignalFromPos(endPos);
            var endPosOutSignal = GetManOutputSignalFromPos(endPos);
            var endPosError = GetEndPosError(endPos);
            return new MechanicMovingProfile<ManipulatorErrors>(endPosOutSignal, startPosSignal, endPosSignal, revers, tormos, endPosError, startPosError);
        }

        private InputSignal<bool> GetManInputSignalFromPos(ManipulatorPosition pos)
        {
            if (pos == ManipulatorPosition.Transport)
                return Position3In;
            if (pos == ManipulatorPosition.Home)
                return Position2In;
            if (pos == ManipulatorPosition.Module)
                return Position1In;
            throw new InvalidOperationException("неверная позиция манипулятора");
        }

        private OutputSignal<bool> GetManOutputSignalFromPos(ManipulatorPosition pos)
        {
            if (pos == ManipulatorPosition.Transport)
                return Position3Out;
            if (pos == ManipulatorPosition.Home)
                return Position2Out;
            if (pos == ManipulatorPosition.Module)
                return Position1Out;
            throw new InvalidOperationException("неверная позиция манипулятора");
        }

        private static ManipulatorErrors GetEndPosError(ManipulatorPosition endPos)
        {
            if (endPos == ManipulatorPosition.Module)
                return ManipulatorErrors.NotComeInPos1;
            if (endPos == ManipulatorPosition.Home)
                return ManipulatorErrors.NotComeInPos2;
            if (endPos == ManipulatorPosition.Transport)
                return ManipulatorErrors.NotComeInPos3;
            throw new InvalidOperationException("Неверная конечная позиция");
        }

        protected override ManipulatorPosition MapState(MechanicsPositions position) => position switch
        {
            MechanicsPositions.Position1 => ManipulatorPosition.Module,
            MechanicsPositions.Position2 => ManipulatorPosition.Home,
            MechanicsPositions.Position3 => ManipulatorPosition.Transport,
            MechanicsPositions.Indefinite => ManipulatorPosition.Indefinite,
            MechanicsPositions.Uncertain => ManipulatorPosition.Uncertain,
            MechanicsPositions.Transition => ManipulatorPosition.Transition,
            _ => ManipulatorPosition.Uncertain
        };

        protected override ManipulatorErrors MapError(MechanicsErrors error) => error switch
        {
            MechanicsErrors.NotInited => ManipulatorErrors.NotInited,
            MechanicsErrors.NotInStartPos => ManipulatorErrors.NotInStartPos,
            MechanicsErrors.IndefinitePos => ManipulatorErrors.IndefinitePos,
            MechanicsErrors.UnsertainPos => ManipulatorErrors.UnsertainPos,
            MechanicsErrors.NotComeInPos1 => ManipulatorErrors.NotComeInPos1,
            MechanicsErrors.NotComeInPos2 => ManipulatorErrors.NotComeInPos2,
            MechanicsErrors.NotComeInPos3 => ManipulatorErrors.NotComeInPos3,
            MechanicsErrors.None => ManipulatorErrors.None,
            _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {error} в ManipulatorErrors")
        };

        protected override MechanicsErrors ToBaseError(ManipulatorErrors error) => error switch
        {
            ManipulatorErrors.NotInited => MechanicsErrors.NotInited,
            ManipulatorErrors.NotInStartPos => MechanicsErrors.NotInStartPos,
            ManipulatorErrors.IndefinitePos => MechanicsErrors.IndefinitePos,
            ManipulatorErrors.UnsertainPos => MechanicsErrors.UnsertainPos,
            ManipulatorErrors.NotComeInPos1 => MechanicsErrors.NotComeInPos1,
            ManipulatorErrors.NotComeInPos2 => MechanicsErrors.NotComeInPos2,
            ManipulatorErrors.NotComeInPos3 => MechanicsErrors.NotComeInPos3,
            ManipulatorErrors.None => MechanicsErrors.None,
            _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {error} в MechanicsErrors")
        };
    }
}
