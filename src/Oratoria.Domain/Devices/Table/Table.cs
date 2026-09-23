using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.Table
{
    public sealed class Table : MechanicDevice<ModuleTablePosition, ModuleTableErrors>
    {
        public Table(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
            : base(deviceId, signals, loggerFactory, settings)
        {
        }

        [DeviceAction("Исходная → Откат")]
        public async Task<bool> FromHomeToRollback(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Из исходной в откат");
            try
            {
                return await Move(ModuleTablePosition.Home, ModuleTablePosition.Rollback, cancellationToken) == ModuleTablePosition.Rollback;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Откат → Обработка")]
        public async Task<bool> FromRollbackToProcessing(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Из отката в обработку");
            try
            {
                return await Move(ModuleTablePosition.Rollback, ModuleTablePosition.Processing, cancellationToken) == ModuleTablePosition.Processing;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Обработка → Откат")]
        public async Task<bool> FromProcessingToRollback(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Из обработки в откат");
            try
            {
                return await Move(ModuleTablePosition.Processing, ModuleTablePosition.Rollback, cancellationToken) == ModuleTablePosition.Rollback;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        [DeviceAction("Откат → Исходная")]
        public async Task<bool> FromRollbackToHome(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Из отката в исходную");
            try
            {
                return await Move(ModuleTablePosition.Rollback, ModuleTablePosition.Home, cancellationToken) == ModuleTablePosition.Home;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        protected override MechanicMovingProfile<ModuleTableErrors> GetMovingProfile(ModuleTablePosition startPos, ModuleTablePosition endPos)
        {
            if (startPos == endPos)
                throw new InvalidOperationException("позиции перемещения стола совпадают");
            if (startPos != ModuleTablePosition.Rollback && endPos != ModuleTablePosition.Rollback)
                throw new InvalidOperationException("неверные позиции перемещения стола");
            var startPosError = ModuleTableErrors.NotInHome;
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
                return ModuleTableErrors.NotCameInRollback;
            if (endPos == ModuleTablePosition.Processing)
                return ModuleTableErrors.NotCameInProcess;
            if (endPos == ModuleTablePosition.Home)
                return ModuleTableErrors.NotCameInHome;
            throw new InvalidOperationException("Неверная конечная позиция");
        }
    }
}
