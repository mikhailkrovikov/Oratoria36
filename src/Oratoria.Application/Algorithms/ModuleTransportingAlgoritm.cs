using Microsoft.Extensions.Logging;
using Oratoria.Domain;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class ModuleTransportingAlgorithm : AlgorithmBase
    {
        private readonly TechnologyModuleContext _context;

        public ModuleTransportingAlgorithm(TechnologyModuleContext context, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger("Транспортировка"))
        {
            _context = context;
        }

        private bool CanLoadPlate() => true;
        private bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate(Plate plate)
        {
            var shutterSub = () => _context.Shutter.Open?.OnSignalChanged += KeepShutterOpen;
            var shutterUnsub = () => _context.Shutter.Open?.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanLoadPlate,
                body => body
                .DoTask(() => _context.Manipulator.FromHomeToTransport())
                .DoTask(() => _context.Manipulator.FromTransportToHome())
                .DoTask(() => TakeFromCarriage(plate))
                .DoTask(() => _context.Shutter.OpenValve())
                .Subscribe(shutterSub, shutterUnsub)
                .DoTask(() => _context.Manipulator.FromHomeToModule())
                .DoTask(() => _context.Table.FromHomeToRollback())
                .DoTask(() => _context.Manipulator.FromModuleToHome())
                .DoTask(() => LeaveInModule(plate))
                .DoTask(() => _context.Table.FromRollbackToProcessing())
                .Unsubscribe(shutterUnsub)
                .DoTask(() => _context.Shutter.CloseValve()));
        }

        public Task<AlgorithmResult> UnloadPlate(Plate plate)
        {
            var shutterSub = () => _context.Shutter.Open?.OnSignalChanged += KeepShutterOpen;
            var shutterUnsub = () => _context.Shutter.Open?.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanUnloadPlate,
                body => body
                .DoTask(() => _context.Table.FromProcessingToRollback())
                .DoTask(() => _context.Shutter.OpenValve())
                .Subscribe(shutterSub, shutterUnsub)
                .DoTask(() => _context.Manipulator.FromHomeToModule())
                .DoTask(() => _context.Table.FromRollbackToHome())
                .DoTask(() => _context.Manipulator.FromModuleToHome())
                .DoTask(() => TakeFromModule(plate))
                .Unsubscribe(shutterUnsub)
                .DoTask(() => _context.Shutter.CloseValve()));
        }

        private static async Task<bool> TakeFromCarriage(Plate plate)
        {
            var ok = await plate.GetState(true);
            if (!ok)
            {
                plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            plate.SetState(PlateStatus.Manipulator);
            return true;
        }

        private static async Task<bool> LeaveInModule(Plate plate)
        {
            var ok = await plate.GetState(false);
            if (!ok)
            {
                plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            plate.SetState(PlateStatus.Module);
            return true;
        }

        private static async Task<bool> TakeFromModule(Plate plate)
        {
            var ok = await plate.GetState(true);
            if (!ok)
            {
                plate.DeviceErrors.AddError(PlateErrors.NotTakenFromModule);
                return false;
            }
            plate.SetState(PlateStatus.Manipulator);
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
}
