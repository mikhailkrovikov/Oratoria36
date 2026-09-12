using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Table
{
    public class Table : MechanicDevice<ModuleTablePosition, ModuleTableErrors>
    {
        public Table(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
            : base(deviceId, signals, loggerFactory, settings)
        {
        }

        [DeviceAction("Исходная → Откат")]
        public async Task<bool> FromHomeToRollback()
        {
            try
            {
                return await Move(ModuleTablePosition.Home, ModuleTablePosition.Rollback) == ModuleTablePosition.Rollback;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Откат → Обработка")]
        public async Task<bool> FromRollbackToProcessing()
        {
            try
            {
                return await Move(ModuleTablePosition.Rollback, ModuleTablePosition.Processing) == ModuleTablePosition.Processing;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Обработка → Откат")]
        public async Task<bool> FromProcessingToRollback()
        {
            try
            {
                return await Move(ModuleTablePosition.Processing, ModuleTablePosition.Rollback) == ModuleTablePosition.Rollback;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Откат → Исходная")]
        public async Task<bool> FromRollbackToHome()
        {
            try
            {
                return await Move(ModuleTablePosition.Rollback, ModuleTablePosition.Home) == ModuleTablePosition.Home;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public override MechanicMovingProfile<ModuleTableErrors> GetMovingProfile(ModuleTablePosition startPos, ModuleTablePosition endPos)
        {
            if (startPos == endPos)
                throw new InvalidOperationException("позиции перемещения стола совпадают");
            if (startPos != ModuleTablePosition.Rollback && endPos != ModuleTablePosition.Rollback)
                throw new InvalidOperationException("неверные позиции перемещения стола");
            var startPosError = ModuleTableErrors.Error2_6;
            var revers = endPos - startPos < 0;
            var tormos = true;
            var startPosSignal = GetTableInputSignalFromPos(startPos);
            var endPosSignal = GetTableInputSignalFromPos(endPos);
            var endPosOutSignal = GetTableOutputSignalFromPos(endPos);
            var endPosError = GetEndPosError(endPos);
            return new MechanicMovingProfile<ModuleTableErrors>(endPosOutSignal, startPosSignal, endPosSignal, revers, tormos, endPosError, startPosError);
        }

        private InputSignal<bool> GetTableInputSignalFromPos(ModuleTablePosition pos)
        {
            if (pos == ModuleTablePosition.Processing)
                return Position3In;
            if (pos == ModuleTablePosition.Rollback)
                return Position2In;
            if (pos == ModuleTablePosition.Home)
                return Position1In;
            throw new InvalidOperationException("неверная позиция ложемента");
        }

        private OutputSignal<bool> GetTableOutputSignalFromPos(ModuleTablePosition pos)
        {
            if (pos == ModuleTablePosition.Processing)
                return Position3Out;
            if (pos == ModuleTablePosition.Rollback)
                return Position2Out;
            if (pos == ModuleTablePosition.Home)
                return Position1Out;
            throw new InvalidOperationException("неверная позиция ложемента");
        }

        private static ModuleTableErrors GetEndPosError(ModuleTablePosition endPos)
        {
            if (endPos == ModuleTablePosition.Rollback)
                return ModuleTableErrors.Error2_3;
            if (endPos == ModuleTablePosition.Processing)
                return ModuleTableErrors.Error2_5;
            if (endPos == ModuleTablePosition.Home)
                return ModuleTableErrors.Error2_4;
            throw new InvalidOperationException("Неверная конечная позиция");
        }

        //protected override ModuleTablePosition MapState(MechanicsPositions position) => position switch
        //{
        //    MechanicsPositions.Position1 => ModuleTablePosition.Home,
        //    MechanicsPositions.Position2 => ModuleTablePosition.Rollback,
        //    MechanicsPositions.Position3 => ModuleTablePosition.Processing,
        //    MechanicsPositions.Indefinite => ModuleTablePosition.Indefinite,
        //    MechanicsPositions.Uncertain => ModuleTablePosition.Uncertain,
        //    MechanicsPositions.Transition => ModuleTablePosition.Transition,
        //    _ => ModuleTablePosition.Uncertain
        //};

        //protected override ModuleTableErrors MapError(MechanicsErrors error) => error switch
        //{
        //    MechanicsErrors.NotInited => ModuleTableErrors.NotInited,
        //    MechanicsErrors.NotInStartPos => ModuleTableErrors.Error2_6,
        //    MechanicsErrors.IndefinitePos => ModuleTableErrors.Error2_1,
        //    MechanicsErrors.UnsertainPos => ModuleTableErrors.Error2_2,
        //    MechanicsErrors.NotComeInPos1 => ModuleTableErrors.Error2_4,
        //    MechanicsErrors.NotComeInPos2 => ModuleTableErrors.Error2_3,
        //    MechanicsErrors.NotComeInPos3 => ModuleTableErrors.Error2_5,
        //    MechanicsErrors.None => ModuleTableErrors.None,
        //    _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {error} в ModuleTableErrors")
        //};

        //protected override MechanicsErrors ToBaseError(ModuleTableErrors error) => error switch
        //{
        //    ModuleTableErrors.NotInited => MechanicsErrors.NotInited,
        //    ModuleTableErrors.Error2_6 => MechanicsErrors.NotInStartPos,
        //    ModuleTableErrors.Error2_1 => MechanicsErrors.IndefinitePos,
        //    ModuleTableErrors.Error2_2 => MechanicsErrors.UnsertainPos,
        //    ModuleTableErrors.Error2_4 => MechanicsErrors.NotComeInPos1,
        //    ModuleTableErrors.Error2_3 => MechanicsErrors.NotComeInPos2,
        //    ModuleTableErrors.Error2_5 => MechanicsErrors.NotComeInPos3,
        //    ModuleTableErrors.None => MechanicsErrors.None,
        //    _ => throw new NotSupportedException($"Не удалось преобразовать ошибку из {error} в MechanicsErrors")
        //};
    }
}
