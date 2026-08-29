using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Oratoria.Domain.Connection.Pollers.Abstractions;
using Timer = System.Timers.Timer;

namespace Oratoria.Application.Connection.Pollers
{
    public class GeneralPoller : INotifyPropertyChanged, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IReadOnlyList<Poller> _pollers;
        private readonly object _connectLocker = new();
        private readonly Stopwatch _sw = new();
        private Timer? _surveyTimer;
        private long _lastPollTime;

        public long LastPollTime
        {
            get => _lastPollTime;
            set
            {
                if (_lastPollTime != value)
                {
                    _lastPollTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public IReadOnlyList<Poller> Pollers => _pollers;

        public GeneralPoller(IEnumerable<Poller> pollers, ILogger<GeneralPoller> logger)
        {
            _pollers = pollers.ToList();
            _logger = logger;
        }

        public Poller? Find(string pollerName)
        {
            return _pollers.FirstOrDefault(p => p.PollerName == pollerName);
        }

        public void StartModule(string pollerName)
        {
            var poller = Find(pollerName)
                ?? throw new ArgumentException($"Поллер '{pollerName}' не найден", nameof(pollerName));
            StartModule(poller);
        }

        public void StartModule(Poller poller)
        {
            poller.Enable();
            _logger.LogInformation("{Poller}: опрос включен", poller.PollerName);
            if (!poller.IsConnected)
                ConnectModule(poller);
        }

        public void StopModule(string pollerName)
        {
            var poller = Find(pollerName)
                ?? throw new ArgumentException($"Поллер '{pollerName}' не найден", nameof(pollerName));
            StopModule(poller);
        }

        public void StopModule(Poller poller)
        {
            poller.Disable();
            _logger.LogInformation("{Poller}: опрос выключен", poller.PollerName);

            var someoneElseUsesConnection = _pollers.Any(p =>
                p != poller &&
                p.IsEnabled &&
                ReferenceEquals(p.Connection, poller.Connection));

            if (!someoneElseUsesConnection && poller.IsConnected)
                DisconnectModule(poller);
        }

        public void StartPoller()
        {
            Task.Run(() =>
            {
                _surveyTimer = new Timer(40);
                _surveyTimer.Elapsed += async (_, _) =>
                {
                    try
                    {
                        if (!Monitor.TryEnter(_connectLocker))
                            return;

                        try
                        {
                            _surveyTimer?.Stop();
                            await TimerTick();
                            _surveyTimer?.Start();
                        }
                        finally
                        {
                            Monitor.Exit(_connectLocker);
                        }
                    }
                    catch
                    {
                    }
                };
                _surveyTimer.Start();
            });
        }

        public void Close()
        {
            _surveyTimer?.Stop();
            _surveyTimer?.Dispose();
            _surveyTimer = null;

            foreach (var poller in _pollers)
                poller.Dispose();
        }

        public void Dispose()
        {
            Close();
        }

        private Task<bool> TimerTick()
        {
            var ret = Poll(out var pollTime);
            LastPollTime = pollTime;
            return Task.FromResult(ret);
        }

        public bool Poll(out long elapsedMilliseconds)
        {
            try
            {
                _sw.Restart();
                foreach (var poller in _pollers)
                {
                    if (poller.IsEnabled)
                        poller.Survey();
                }
                _sw.Stop();

                if (_sw.ElapsedMilliseconds > 0)
                    _logger.LogDebug("Время опроса: {Elapsed} мс", _sw.ElapsedMilliseconds);

                elapsedMilliseconds = _sw.ElapsedMilliseconds;
                return true;
            }
            catch
            {
                elapsedMilliseconds = 0;
                return false;
            }
        }

        public bool ConnectModule(Poller poller)
        {
            Task.Run(async () =>
            {
                Monitor.Enter(_connectLocker);
                try
                {
                    Thread.Sleep(300);
                    var startRet = await poller.Connect();
                    if (!startRet)
                        DisconnectModule(poller);
                    else
                        poller.Survey();
                }
                finally
                {
                    Monitor.Exit(_connectLocker);
                }
            });
            return true;
        }

        public void DisconnectModule(Poller poller)
        {
            poller.Disconnect();
            _logger.LogDebug("{Poller} отключен", poller.PollerName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
