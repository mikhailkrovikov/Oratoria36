using Oratoria.Domain;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Algorithms
{
    public class ModuleTransportingAlgorithm : AlgorithmBase
    {
        private readonly TechnologyModuleContext _context;
        public ModuleTransportingAlgorithm(TechnologyModuleContext context)
        {
            _context = context;
        }

        private bool CanLoadPlate() => true;
        private bool CanUnloadPlate() => true;

        public Task<AlgorithmResult> LoadPlate(Plate plate, CancellationToken cancellationToken = default)
        {
            var shutterSub = () => _context.Shutter.OpenSignal?.OnSignalChanged += KeepShutterOpen;
            var shutterUnsub = () => _context.Shutter.OpenSignal?.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanLoadPlate,
                body => body
                    .DoTask(_context.Manipulator.FromHomeToTransport)
                    .DoTask(_context.Manipulator.FromTransportToHome)
                    .DoTask(ct => TakeFromCarriage(plate, ct))
                    .DoTask(_context.Shutter.OpenValve)
                    .Subscribe(shutterSub, shutterUnsub)
                    .DoTask(_context.Manipulator.FromHomeToModule)
                    .DoTask(_context.Table.FromHomeToRollback)
                    .DoTask(_context.Manipulator.FromModuleToHome)
                    .DoTask(ct => LeaveInModule(plate, ct))
                    .DoTask(_context.Table.FromRollbackToProcessing)
                    .Unsubscribe(shutterUnsub)
                    .DoTask(_context.Shutter.CloseValve),
                cancellationToken);
        }

        public Task<AlgorithmResult> UnloadPlate(Plate plate, CancellationToken cancellationToken = default)
        {
            var shutterSub = () => _context.Shutter.OpenSignal?.OnSignalChanged += KeepShutterOpen;
            var shutterUnsub = () => _context.Shutter.OpenSignal?.OnSignalChanged -= KeepShutterOpen;

            return Execute(CanUnloadPlate,
                body => body
                    .DoTask(_context.Table.FromProcessingToRollback)
                    .DoTask(_context.Shutter.OpenValve)
                    .Subscribe(shutterSub, shutterUnsub)
                    .DoTask(_context.Manipulator.FromHomeToModule)
                    .DoTask(_context.Table.FromRollbackToHome)
                    .DoTask(_context.Manipulator.FromModuleToHome)
                    .DoTask(ct => TakeFromModule(plate, ct))
                    .Unsubscribe(shutterUnsub)
                    .DoTask(_context.Shutter.CloseValve),
                cancellationToken);
        }

        private static async Task<bool> TakeFromCarriage(
            Plate plate,
            CancellationToken cancellationToken)
        {
            var ok = await plate.GetState(true, cancellationToken);
            if (!ok)
            {
                plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            plate.SetState(PlateStatus.Manipulator);
            return true;
        }

        private static async Task<bool> LeaveInModule(
            Plate plate,
            CancellationToken cancellationToken)
        {
            var ok = await plate.GetState(false, cancellationToken);
            if (!ok)
            {
                plate.DeviceErrors.AddError(PlateErrors.NotTakenFromTransport);
                return false;
            }
            plate.SetState(PlateStatus.Module);
            return true;
        }

        private static async Task<bool> TakeFromModule(
            Plate plate,
            CancellationToken cancellationToken)
        {
            var ok = await plate.GetState(true, cancellationToken);
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
            if (_context.Shutter.OpenSignal != null && !_context.Shutter.OpenSignal.Value)
            {
                _context.Shutter.OpenSignal.Value = true;
            }
        }
    }
}
