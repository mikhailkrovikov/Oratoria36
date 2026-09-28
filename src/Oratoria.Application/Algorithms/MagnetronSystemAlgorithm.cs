using Oratoria.Domain.Algorithms;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Algorithms
{
    public class MagnetronSystemAlgorithm : AlgorithmBase
    {
        private readonly TechnologyModuleContext _context;
        public MagnetronSystemAlgorithm(TechnologyModuleContext context)
        {
            _context = context;
        }

        public bool CanStartAllMagns() => true;
        public bool CanStopAllMagns() => true;

        public Task<AlgorithmResult> StartAllMagnetrons(
            double setPoint1,
            double setPoint2,
            double setPoint3,
            CancellationToken cancellationToken = default)
        {
            return Execute(CanStartAllMagns,
                body => body
                    .Parallel(
                        ct => _context.Magnetron1.TurnOn(setPoint1, ct),
                        ct => _context.Magnetron2.TurnOn(setPoint2, ct),
                        ct => _context.Magnetron3.TurnOn(setPoint3, ct))
                    .DoTask(WaitRotation),
                cancellationToken);
        }

        public Task<AlgorithmResult> StopAllMagnetrons(
            CancellationToken cancellationToken = default)
        {
            return Execute(
                CanStopAllMagns,
                body => body.DoTask(StopThreeMagns),
                cancellationToken);
        }

        private async Task<bool> StopThreeMagns(
            CancellationToken cancellationToken)
        {
            var results = await Task.WhenAll(
                _context.Magnetron1.TurnOff(cancellationToken),
                _context.Magnetron2.TurnOff(cancellationToken),
                _context.Magnetron3.TurnOff(cancellationToken));

            if (!results.All(result => result))
                return false;

            return await WaitRotationOff(cancellationToken);
        }

        private async Task<bool> WaitRotation(
            CancellationToken cancellationToken)
        {
            _context.Magnetron1.RotationOn.Value = true;

            cancellationToken.ThrowIfCancellationRequested();

            if (_context.Magnetron1.IsRotating.Value)
                return true;

            var result = await EventWaiter.WaitEvent(
                nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                _context.Magnetron1.IsRotating,
                (bool value) => value,
                _context.Magnetron1.TimeForError.Value,
                cancellationToken);

            if (result)
                return true;

            Reason = "Ошибка запуска вращения";

            await StopThreeMagns(cancellationToken);

            _context.Magnetron1.RotationOn.Value = false;
            return false;
        }

        private async Task<bool> WaitRotationOff(CancellationToken cancellationToken)
        {
            _context.Magnetron1.RotationOn.Value = false;
            cancellationToken.ThrowIfCancellationRequested();
            if (_context.Magnetron1.IsRotating.Value)
            {
                await EventWaiter.WaitEvent(
                    nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                    _context.Magnetron1.IsRotating,
                    (bool x) => !x,
                    _context.Magnetron1.TimeForError.Value,
                    cancellationToken);
            }
            return true;
        }
    }
}
