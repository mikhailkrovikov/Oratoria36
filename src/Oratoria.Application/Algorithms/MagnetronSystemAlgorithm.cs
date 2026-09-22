using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Algorithms;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Algorithms
{
    public class MagnetronSystemAlgorithm : AlgorithmBase
    {
        private readonly TechnologyModuleContext _context;

        public MagnetronSystemAlgorithm(TechnologyModuleContext context, ILoggerFactory loggerFactory) 
            : base(loggerFactory.CreateLogger("Магнетронная система"))
        {
            _context = context;
        }

        public bool CanStartAllMagns() => true;
        public bool CanStopAllMagns() => true;

        public Task<AlgorithmResult> StartAllMagnetrons(double setPoint1, double setPoint2, double setPoint3)
        {
            var parallel = new List<Func<Task<bool>>>();
            parallel.Add(() => _context.Magnetron1.TurnOn(setPoint1));
            parallel.Add(() => _context.Magnetron2.TurnOn(setPoint2));
            parallel.Add(() => _context.Magnetron3.TurnOn(setPoint3));

            return Execute(CanStartAllMagns, 
                body => body
                .DoParallelTasks(parallel.ToArray())
                .DoTask(WaitRotation));
        }

        public Task<AlgorithmResult> StopAllMagnetrons()
        {
            return Execute(CanStopAllMagns, StopThreeMagnsParallel);
        }

        private AlgorithmBody StopThreeMagnsParallel(AlgorithmBody body)
            => body
                .DoParallelTasks(
                    _context.Magnetron1.TurnOff,
                    _context.Magnetron2.TurnOff,
                    _context.Magnetron3.TurnOff)
                .DoTask(WaitRotationOff);


        private async Task<bool> WaitRotation()
        {
            _context.Magnetron1.RotationOn.Value = true;
            Token.ThrowIfCancellationRequested();
            if (_context.Magnetron1.IsRotating.Value)
                return true;
            var res = await EventWaiter.WaitEvent(
                nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                _context.Magnetron1.IsRotating,
                (bool x) => x,
                _context.Magnetron1.TimeForError.Value,
                Token);
            if (!res)
            {
                Logger.LogError("Ошибка запуска вращения");
                await StopThreeMagnsParallel(new AlgorithmBody(this, Token)).Start();
                _context.Magnetron1.RotationOn.Value = false;
                return false;
            }
            return true;
        }

        private async Task<bool> WaitRotationOff()
        {
            _context.Magnetron1.RotationOn.Value = false;
            Token.ThrowIfCancellationRequested();
            if (_context.Magnetron1.IsRotating.Value)
            {
                await EventWaiter.WaitEvent(
                    nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                    _context.Magnetron1.IsRotating,
                    (bool x) => !x,
                    _context.Magnetron1.TimeForError.Value,
                    Token);
            }
            return true;
        }
    }
}
