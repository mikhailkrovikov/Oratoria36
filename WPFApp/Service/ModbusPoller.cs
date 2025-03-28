using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Oratoria36.Models;
using Oratoria36.Models.Signals;

namespace Oratoria36.Service
{
    /// <summary>
    /// Сервис для работы с сигналами Modbus.
    /// Отвечает за опрос и запись сигналов.
    /// </summary>
    public class ModbusPoller
    {
        private static readonly Logger _logger = LogManager.GetLogger("ModbusPoller");
        private static ModbusPoller _instance;
        public static ModbusPoller Instance => _instance ??= new ModbusPoller();

        private readonly Dictionary<ModuleConfig, List<Signal>> _moduleSignals = new();

        private CancellationTokenSource _pollingCts;
        private Task _pollingTask;
        private bool _isPolling;
        private readonly int _pollingIntervalMs = 100;

        private ModbusPoller() { }

        /// <summary>
        /// Запускает опрос сигналов
        /// </summary>
        public void StartPolling()
        {
            if (_isPolling) return;

            _isPolling = true;
            _pollingCts = new CancellationTokenSource();
            _pollingTask = Task.Run(() => PollSignalsAsync(_pollingCts.Token));
            _logger.Info("Опрос сигналов запущен");
        }

        /// <summary>
        /// Останавливает опрос сигналов
        /// </summary>
        public void StopPolling()
        {
            if (!_isPolling) return;

            _pollingCts?.Cancel();
            try
            {
                _pollingTask?.Wait(1000);
            }
            catch (AggregateException) { }

            _isPolling = false;
            _logger.Info("Опрос сигналов остановлен");
        }

        /// <summary>
        /// Регистрирует модуль и его сигналы
        /// </summary>
        public void RegisterModule(ModuleConfig config, List<Signal> signals)
        {
            if (config == null)
            {
                _logger.Warn("Попытка регистрации null-модуля");
                return;
            }

            if (!_moduleSignals.ContainsKey(config))
            {
                _moduleSignals[config] = new List<Signal>();
            }

            if (signals != null)
            {
                _moduleSignals[config].AddRange(signals);
            }

            _logger.Info($"Зарегистрирован модуль {config.ModuleId} с {signals?.Count ?? 0} сигналами");
        }

        /// <summary>
        /// Вызывается при подключении модуля
        /// </summary>
        public void OnModuleConnected(ModuleConfig config)
        {
            _logger.Info($"Модуль {config.ModuleId} подключен и готов к опросу");
        }

        /// <summary>
        /// Вызывается при отключении модуля
        /// </summary>
        public void OnModuleDisconnected(ModuleConfig config)
        {
            _logger.Info($"Модуль {config.ModuleId} отключен, опрос прекращен");

            // Если у нас есть сигналы для этого модуля, помечаем их как недействительные
            if (_moduleSignals.ContainsKey(config))
            {
                foreach (var signal in _moduleSignals[config])
                {
                    signal.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Записывает значение в выходной сигнал
        /// </summary>
        public async Task WriteSignalAsync(OutputSignal signal)
        {
            if (signal == null)
            {
                _logger.Warn("Попытка записи в null-сигнал");
                return;
            }
            ModuleConfig targetModule = null;
            foreach (var kvp in _moduleSignals)
            {
                if (kvp.Value.Contains(signal))
                {
                    targetModule = kvp.Key;
                    break;
                }
            }

            if (targetModule == null || !targetModule.IsConnected)
            {
                _logger.Warn($"Попытка записи в сигнал {signal.Name}, но модуль не найден или не подключен");
                return;
            }

            try
            {
                await Task.Run(() => targetModule.Master.WriteSingleCoil(0, signal.Channel, signal.Value));
                _logger.Debug($"Записано {signal.Value} в {signal.Name}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка записи в {signal.Name}");
            }
        }

        /// <summary>
        /// Опрашивает сигналы всех модулей
        /// </summary>
        private async Task PollSignalsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var kvp in _moduleSignals)
                {
                    var module = kvp.Key;
                    var signals = kvp.Value;

                    if (!module.IsConnected)
                        continue;

                    await PollModuleSignalsAsync(module, signals, token);
                }

                try
                {
                    await Task.Delay(_pollingIntervalMs, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Опрашивает сигналы конкретного модуля
        /// </summary>
        private async Task PollModuleSignalsAsync(ModuleConfig module, List<Signal> signals, CancellationToken token)
        {
            if (token.IsCancellationRequested || !module.IsConnected) return;

            try
            {
                var inputSignals = signals.OfType<InputSignal>().ToList();
                var outputSignals = signals.OfType<OutputSignal>().ToList();

                if (inputSignals.Any())
                {
                    await PollInputSignalsAsync(module, inputSignals, token);
                }
                if (outputSignals.Any())
                {
                    await PollOutputSignalsAsync(module, outputSignals, token);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при опросе сигналов для модуля {module.ModuleId}");

                foreach (var signal in signals)
                {
                    signal.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Опрашивает входные сигналы
        /// </summary>
        private async Task PollInputSignalsAsync(ModuleConfig module, List<InputSignal> signals, CancellationToken token)
        {
            if (signals.Count == 0 || token.IsCancellationRequested || !module.IsConnected) return;

            try
            {
                ushort minChannel = signals.Min(s => s.Channel);
                ushort maxChannel = signals.Max(s => s.Channel);
                ushort count = (ushort)(maxChannel - minChannel + 1);

                bool[] inputs = await Task.Run(() => module.Master.ReadInputs(0, minChannel, count), token);

                foreach (var signal in signals)
                {
                    int index = signal.Channel - minChannel;
                    if (index >= 0 && index < inputs.Length)
                    {
                        signal.Value = inputs[index];
                        signal.IsValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка чтения входных сигналов модуля {module.ModuleId}");
                foreach (var signal in signals)
                {
                    signal.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Опрашивает выходные сигналы
        /// </summary>
        private async Task PollOutputSignalsAsync(ModuleConfig module, List<OutputSignal> signals, CancellationToken token)
        {
            if (signals.Count == 0 || token.IsCancellationRequested || !module.IsConnected) return;

            try
            {
                ushort minChannel = signals.Min(s => s.Channel);
                ushort maxChannel = signals.Max(s => s.Channel);
                ushort count = (ushort)(maxChannel - minChannel + 1);

                bool[] outputs = await Task.Run(() => module.Master.ReadCoils(0, minChannel, count), token);

                foreach (var signal in signals)
                {
                    int index = signal.Channel - minChannel;
                    if (index >= 0 && index < outputs.Length)
                    {
                        signal.UpdateValueWithoutWrite(outputs[index]);
                        signal.IsValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка чтения выходных сигналов модуля {module.ModuleId}");
                foreach (var signal in signals)
                {
                    signal.IsValid = false;
                }
            }
        }
    }
}
