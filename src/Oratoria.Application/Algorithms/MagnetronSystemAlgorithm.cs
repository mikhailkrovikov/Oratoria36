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

    //public class MagnetronSystemAlgorith
    //{
    //    private readonly CancellationTokenSource _source = new();
    //    private readonly ILogger _logger;
    //    private readonly TechnologyModuleContext _context;

    //    public MagnetronSystemAlgorith(TechnologyModuleContext context, ILoggerFactory loggerFactory)
    //    {
    //        _context = context;
    //        _logger = loggerFactory.CreateLogger("Магнетронная система");
    //    }

    //    public async Task<bool> StartAllMagns(double setPoint1, double setPoint2, double setPoint3)
    //    {
    //        try
    //        {
    //            var token = _source.Token;
    //            var tasks = new List<Task<bool>>();

    //            if (setPoint1 != 0)
    //                tasks.Add(_context.Magnetron1.TurnOn(setPoint1));
    //            if (setPoint2 != 0)
    //                tasks.Add(_context.Magnetron2.TurnOn(setPoint2));
    //            if (setPoint3 != 0)
    //                tasks.Add(_context.Magnetron3.TurnOn(setPoint3));

    //            if (tasks.Count == 0)
    //                return true;

    //            await Task.WhenAll(tasks);
    //            foreach (var task in tasks)
    //            {
    //                if (!task.Result)
    //                    return false;
    //            }

    //            _context.Magnetron1.RotationOn.Value = true;

    //            token.ThrowIfCancellationRequested();
    //            if (!_context.Magnetron1.IsRotating.Value)
    //            {
    //                var res = await EventWaiter.WaitEvent(
    //                    nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
    //                    _context.Magnetron1.IsRotating,
    //                    (bool x) => x,
    //                     _context.Magnetron1.TimeForError.Value, token);

    //                token.ThrowIfCancellationRequested();

    //                if (!res)
    //                {
    //                    _logger.LogError($"Ошибка запуска вращения");
    //                    await StopAllMagns();
    //                    _context.Magnetron1.RotationOn.Value = false;
    //                    return false;
    //                }
    //            }

    //            return tasks.All(t => t.Result);
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            _logger.LogInformation($"отмена запуска всех магнетронов");
    //            return false;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            return false;
    //        }
    //    }

    //    public async Task<bool> StopAllMagns()
    //    {
    //        try
    //        {
    //            var token = _source.Token;

    //            await Task.WhenAll(
    //                _context.Magnetron1.TurnOff(), 
    //                _context.Magnetron2.TurnOff(), 
    //                _context.Magnetron3.TurnOff());
    
    //            _context.Magnetron1.RotationOn.Value = false;
    //            token.ThrowIfCancellationRequested();

    //            if (_context.Magnetron1.IsRotating.Value)
    //            {
    //                await EventWaiter.WaitEvent(
    //                    nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
    //                    _context.Magnetron1.IsRotating,
    //                    (bool x) => !x,
    //                    _context.Magnetron1.TimeForError.Value, token);
    //            }

    //            return true;
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            _logger.LogInformation($"Отмена остановки всех магнетронов");
    //            return false;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            return false;
    //        }
    //    }
    //}
}
