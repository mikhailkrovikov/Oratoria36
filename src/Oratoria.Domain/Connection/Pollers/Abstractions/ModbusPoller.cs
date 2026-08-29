using Microsoft.Extensions.Logging;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Connection.Pollers.Abstractions
{
    public class ModbusPoller : Poller
    {
        private readonly IModuleSignals _signals;
        private readonly ILogger _logger;

        public ModbusPoller(IConnectionConfig config, IModuleSignals signals, ILoggerFactory loggerFactory, string name)
            : base(config, name)
        {
            _signals = signals;
            _logger = loggerFactory.CreateLogger("Modbus опрос");
        }

        public override void Survey()
        {
            try
            {
                sw.Restart();

                foreach (var signal in _signals.DISignals)
                {
                    if (!signal.GetNewValue())
                        throw new Exception($"Ошибка при чтении дискретного входа {PollerName}/{signal.Name}/{signal.PinNumber}");
                }

                foreach (var signal in _signals.AISignals)
                {
                    if (!signal.GetNewValue())
                        throw new Exception($"Ошибка при чтении аналогового входа {PollerName}/{signal.Name}/{signal.PinNumber}");
                }

                sw.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
