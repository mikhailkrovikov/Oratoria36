using Oratoria36.Models.Signals;


namespace Oratoria36.Models
{
    public partial class ModuleConfig
    {
        private class Poller
        {
            private readonly ModuleConfig _module;
            private CancellationTokenSource _pollingCts;
            private Task _pollingTask;
            private int _pollingIntervalMs = 100;
            private readonly List<Signal> _signals = new List<Signal>();

            public Poller(ModuleConfig module)
            {
                _module = module;
                Start();
            }

            public void RegisterSignal(Signal signal)
            {
                if (signal != null && !_signals.Contains(signal))
                {
                    _signals.Add(signal);
                }
            }

            public void Start()
            {
                if (_pollingTask != null && !_pollingTask.IsCompleted) return;

                _pollingCts = new CancellationTokenSource();
                _pollingTask = Task.Run(async () => await PollSignalsAsync(_pollingCts.Token));
            }

            public void Stop()
            {
                _pollingCts?.Cancel();
            }

            private async Task PollSignalsAsync(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_module.IsConnected)
                    {
                        foreach (var signal in _signals)
                        {
                            try
                            {
                                await signal.ReadValueAsync(_module);
                            }
                            catch
                            {
                            }
                        }
                    }
                    await Task.Delay(_pollingIntervalMs, cancellationToken);
                }
            }
        }
    }
}
