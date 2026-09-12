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
            Logger.LogInformation("Из исходной в транспорт");
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
            Logger.LogInformation("Из модуля в исходную");
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
            Logger.LogInformation("Из исходной в модуль");
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
            Logger.LogInformation("Из транспорта в исходную");
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
    }
}
