using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NLog;
using Oratoria.Infrastructure;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Oratoria.Application.Algorithms
{
    public class MagnetronSystemAlgorith
    {
        public const int TIME_FOR_MAGN_AND_HEATER_TURNING = 1000;
        private readonly CancellationTokenSource _source = new();
        private readonly ILogger _logger;
        private readonly TechnologyModuleContext _context;

        public MagnetronSystemAlgorith(TechnologyModuleContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("Магнетронная система");
        }

        public async Task<bool> StartAllMagns(double setPoint1, double setPoint2, double setPoint3)
        {
            try
            {
                var token = _source.Token;
                var tasks = new List<Task<bool>>();

                if (setPoint1 != 0)
                    tasks.Add(_context.Magnetron1.TurnOn(setPoint1));
                if (setPoint2 != 0)
                    tasks.Add(_context.Magnetron2.TurnOn(setPoint2));
                if (setPoint3 != 0)
                    tasks.Add(_context.Magnetron3.TurnOn(setPoint3));

                if (tasks.Count == 0)
                    return true;

                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    if (!task.Result)
                        return false;
                }

                _context.Magnetron1.RotationOn.Value = true;

                token.ThrowIfCancellationRequested();
                if (!_context.Magnetron1.IsRotating.Value)
                {
                    var res = await EventWaiter.WaitEvent(
                        nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                        _context.Magnetron1.IsRotating,
                        (bool x) => x,
                        TIME_FOR_MAGN_AND_HEATER_TURNING, token);

                    token.ThrowIfCancellationRequested();

                    if (!res)
                    {
                        _logger.LogError($"Ошибка запуска вращения");
                        await StopAllMagns();
                        _context.Magnetron1.RotationOn.Value = false;
                        return false;
                    }
                }

                return tasks.All(t => t.Result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"отмена запуска всех магнетронов");
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> StopAllMagns()
        {
            try
            {
                var token = _source.Token;

                await Task.WhenAll(
                    _context.Magnetron1.TurnOff(), 
                    _context.Magnetron2.TurnOff(), 
                    _context.Magnetron3.TurnOff());
    
                _context.Magnetron1.RotationOn.Value = false;
                token.ThrowIfCancellationRequested();

                if (_context.Magnetron1.IsRotating.Value)
                {
                    await EventWaiter.WaitEvent(
                        nameof(_context.Magnetron1.IsRotating.OnSignalChanged),
                        _context.Magnetron1.IsRotating,
                        (bool x) => !x,
                        TIME_FOR_MAGN_AND_HEATER_TURNING, token);
                }

                return true;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Отмена остановки всех магнетронов");
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
