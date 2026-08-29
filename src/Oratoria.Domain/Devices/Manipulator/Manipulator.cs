using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Manipulator
{
    public class Manipulator : MechanicDevice<ManipulatorPosition, ManipulatorErrors>
    {
        public Manipulator(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory) : base(deviceId, signals, loggerFactory)
        {         
        }


        public async Task<bool> FromHomeToTransport()
        {
            return await Move(ManipulatorPosition.Home, ManipulatorPosition.Transport) == ManipulatorPosition.Transport;
        }

        public async Task<bool> FromModuleToHome()
        {
            return await Move(ManipulatorPosition.Module, ManipulatorPosition.Home) == ManipulatorPosition.Home;
        }

        public async Task<bool> FromHomeToModule()
        {
            return await Move(ManipulatorPosition.Home, ManipulatorPosition.Module) == ManipulatorPosition.Module;
        }

        public async Task<bool> FromTransportToHome()
        {
            return await Move(ManipulatorPosition.Transport, ManipulatorPosition.Home) == ManipulatorPosition.Home;
        }

        public override MechanicMovingProfile<ManipulatorErrors> GetMovingProfile(ManipulatorPosition startPos, ManipulatorPosition endPos)
        {
            if (startPos == endPos)
                throw new Exception("позиции перемещения манипулятора совпадают");
            if (startPos != ManipulatorPosition.Home && endPos != ManipulatorPosition.Home)
                throw new Exception("неверные позиции перемещения манипулятора");
            var startPosError = ManipulatorErrors.Error1_1;
            var revers = endPos - startPos < 0;
            var tormos = true;
            InputSignal<bool> startPosSignal = GetManInputSignalFromPos(startPos);
            InputSignal<bool> endPosSignal = GetManInputSignalFromPos(endPos);
            OutputSignal<bool> endPosOutSignal = GetManOutputSignalFromPos(endPos);
            ManipulatorErrors endPosError = GetEndPosError(startPos, endPos);
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
            throw new Exception("неверная позиция манипулятора");
        }

        private OutputSignal<bool> GetManOutputSignalFromPos(ManipulatorPosition pos)
        {
            if (pos == ManipulatorPosition.Transport)
                return Position3Out;
            if (pos == ManipulatorPosition.Home)
                return Position2Out;
            if (pos == ManipulatorPosition.Module)
                return Position1Out;
            throw new Exception("неверная позиция манипулятора");
        }

        private static ManipulatorErrors GetEndPosError(ManipulatorPosition startPos, ManipulatorPosition endPos)
        {
            if (startPos == ManipulatorPosition.Home)
            {
                if (endPos == ManipulatorPosition.Transport)
                    return ManipulatorErrors.Error1_6;
                if (endPos == ManipulatorPosition.Module)
                    return ManipulatorErrors.Error1_4;
            }
            if (endPos == ManipulatorPosition.Home)
            {
                if (startPos == ManipulatorPosition.Transport)
                    return ManipulatorErrors.Error1_7;
                if (startPos == ManipulatorPosition.Module)
                    return ManipulatorErrors.Error1_5;
            }
            throw new Exception("Неверные начальная или конечная позиция");
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
            MechanicsErrors.NotInStartPos => ManipulatorErrors.Error1_1,
            MechanicsErrors.IndefinitePos => ManipulatorErrors.Error1_2,
            MechanicsErrors.UnsertainPos => ManipulatorErrors.Error1_3,
            MechanicsErrors.NotComeInPos1 => ManipulatorErrors.Error1_4,
            MechanicsErrors.NotComeInPos2 => ManipulatorErrors.Error1_5,
            MechanicsErrors.NotComeInPos3 => ManipulatorErrors.Error1_6,
            MechanicsErrors.None => ManipulatorErrors.None,
            _ => ManipulatorErrors.Error1_1
        };

        protected override MechanicsErrors ToBaseError(ManipulatorErrors error) => error switch
        {
            ManipulatorErrors.NotInited => MechanicsErrors.NotInited,
            ManipulatorErrors.Error1_1 => MechanicsErrors.NotInStartPos,
            ManipulatorErrors.Error1_2 => MechanicsErrors.IndefinitePos,
            ManipulatorErrors.Error1_3 => MechanicsErrors.UnsertainPos,
            ManipulatorErrors.Error1_4 => MechanicsErrors.NotComeInPos1,
            ManipulatorErrors.Error1_5 => MechanicsErrors.NotComeInPos2,
            ManipulatorErrors.Error1_6 => MechanicsErrors.NotComeInPos3,
            ManipulatorErrors.Error1_7 => MechanicsErrors.NotComeInPos2,
            ManipulatorErrors.None => MechanicsErrors.None,
            _ => MechanicsErrors.NotInEndPos
        };
    }
}
