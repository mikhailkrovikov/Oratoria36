using Microsoft.Extensions.Logging;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Algorithms
{
    public class ModuleTransportingAlgorithm : AlgorithmBase
    {
        private readonly TechnologyModuleContext _context;
        public Plate Plate;

        public ModuleTransportingAlgorithm(TechnologyModuleContext context, Plate plate, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger("Транспортировка"))
        {
            _context = context;
            Plate = plate;        
        }

        public bool CanLoadPlate() => true;
        public bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate()
        {
            var unsub = () => _context.Shutter.Open.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanLoadPlate,
                body => body
                .DoTask(() => _context.Manipulator.FromHomeToTransport())
                .DoTask(() => _context.Manipulator.FromTransportToHome())
                .DoTask(TakeFromCarriage)
                .DoTask(() => _context.Shutter.OpenValve())
                .Subscribe(() => _context.Shutter.Open.OnSignalChanged += KeepShutterOpen, unsub)
                .DoTask(() => _context.Manipulator.FromHomeToModule())
                .DoTask(() => _context.Table.FromHomeToRollback())
                .DoTask(() => _context.Manipulator.FromModuleToHome())
                .DoTask(LeaveInModule)
                .DoTask(() => _context.Table.FromRollbackToProcessing())
                .Unsubscribe(unsub)
                .DoTask(() => _context.Shutter.CloseValve()));
        }


        public Task<AlgorithmResult> UnloadPlate()
        {
            var unsub = () => _context.Shutter.Open.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanUnloadPlate,
                body => body
                .DoTask(() => _context.Table.FromProcessingToRollback())
                .DoTask(() => _context.Shutter.OpenValve())
                .Subscribe(() => _context.Shutter.Open.OnSignalChanged += KeepShutterOpen, unsub)
                .DoTask(() => _context.Manipulator.FromHomeToModule())
                .DoTask(() => _context.Table.FromRollbackToHome())
                .DoTask(() => _context.Manipulator.FromModuleToHome())
                .DoTask(TakeFromModule)
                .Unsubscribe(unsub)
                .DoTask(() => _context.Shutter.CloseValve()));
        }

        private async Task<bool> TakeFromCarriage()
        {
            var ok = await Plate.GetState(true);
            if (!ok)
            {
                Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            Plate.SetState(PlateStatus.Manipulator);
            return true;
        }

        private async Task<bool> LeaveInModule()
        {
            var ok = await Plate.GetState(false);
            if (!ok)
            {
                Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            Plate.SetState(PlateStatus.Module);
            return true;
        }

        private async Task<bool> TakeFromModule()
        {
            var ok = await Plate.GetState(true);
            if (!ok)
            {
                Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromModule);
                return false;
            }
            Plate.SetState(PlateStatus.Manipulator);
            return true;
        }
        private void KeepShutterOpen(bool value)
        {
            if (_context.Shutter.Open != null && !_context.Shutter.Open.Value)
            {
                Logger.LogWarning("Неожиданное закрытие ЩЗ, принудительное открытие");
                _context.Shutter.Open.Value = true;
            }
        }
    }

    public class ModuleTransportingAlgoritm
    {
        public Plate Plate;
        private readonly ILogger _logger;
        private CancellationTokenSource _source = new();
        private readonly TechnologyModuleContext _context;

        public ModuleTransportingAlgoritm(TechnologyModuleContext context, Plate plate, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("Транспортировка");
            Plate = plate;
        }

        public async Task<bool> LoadPlate()
        {
            _logger.LogInformation("Загрузка пластины");
            var token = _source.Token;
            try
            {
                var res = false;

                res = await _context.Manipulator.FromHomeToTransport();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                res = await _context.Manipulator.FromTransportToHome();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                res = await Plate.GetState(true);
                if (!res)
                {
                    Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                    return res;
                }
                Plate.SetState(PlateStatus.Manipulator);

                res = await _context.Shutter.OpenValve();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                _context.Shutter.Open?.OnSignalChanged += BlockShutter;

                res = await _context.Manipulator.FromHomeToModule();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                res = await _context.Table.FromHomeToRollback();
                token.ThrowIfCancellationRequested();
                if (!res) return res;


                res = await _context.Manipulator.FromModuleToHome();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                res = await Plate.GetState(false);
                if (!res)
                {
                    Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                    return res;
                }
                Plate.SetState(PlateStatus.Module);

                res = await _context.Table.FromRollbackToProcessing();
                token.ThrowIfCancellationRequested();
                if (!res) return res;

                _context.Shutter.Open?.OnSignalChanged -= BlockShutter;
                res = await _context.Shutter.CloseValve();
                if (!res) return res;

                _logger.LogInformation("Загрузка пластины успешно завершена");
                return res;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Операция загрузки прервана");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> UnloadPlate()
        {
            _logger.LogInformation("Выгрузка пластины");
            var token = _source.Token;
            try
            {
                var res = false;

                token.ThrowIfCancellationRequested();
                res = await _context.Table.FromProcessingToRollback();
                if (!res) return res;

                token.ThrowIfCancellationRequested();
                res = await _context.Shutter.OpenValve();
                if (!res) return res;

                _context.Shutter.Open?.OnSignalChanged += BlockShutter;

                token.ThrowIfCancellationRequested();
                res = await _context.Manipulator.FromHomeToModule();
                if (!res) return res;

                token.ThrowIfCancellationRequested();
                res = await _context.Table.FromRollbackToHome();
                if (!res) return res;

                token.ThrowIfCancellationRequested();
                res = await _context.Manipulator.FromModuleToHome();
                if (!res) return res;

                res = await Plate.GetState(true);
                if (!res)
                {
                    Plate.DeviceErrors.AddError(PlateErrors.NotTakenFromModule);
                    return res;
                }
                Plate.SetState(PlateStatus.Manipulator);
                return res;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Операция выгрузки прервана");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private void BlockShutter(bool value)
        {
            if (!_context.Shutter.Open.Value)
            {
                _logger.LogWarning("Неожидаемое закрытие ЩЗ, принудительное открытие");
                _context.Shutter.Open.Value = true;
            }
        }
    }
}
