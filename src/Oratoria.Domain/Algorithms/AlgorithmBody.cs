using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Algorithms
{
    public class AlgorithmBody
    {
        private readonly AlgorithmBase _owner;
        private readonly CancellationToken _token;
        private readonly List<Func<Task<AlgorithmResult>>> _actions = new();
        private readonly List<Action> _subscriptions = new();
        public AlgorithmBody(AlgorithmBase owner, CancellationToken token)
        {
            _owner = owner;
            _token = token;
        }

        public void Schedule(Func<Task<AlgorithmResult>> func)
        {
            _actions.Add(func);
        }

        public AlgorithmBody DoTask(Func<Task<bool>> action)
        {
            Schedule(async () =>
            {
                _token.ThrowIfCancellationRequested();
                var ok = await action();
                return ok ? AlgorithmResult.Success() : AlgorithmResult.Fail(_owner);
            });
            return this;
        }

        public AlgorithmBody DoParallelTasks(params Func<Task<bool>>[] actions)
        {
            Schedule(async () =>
            {
                _token.ThrowIfCancellationRequested();
                var results = await Task.WhenAll(actions.Select(a => a()));
                return results.All(ok => ok)
                    ? AlgorithmResult.Success()
                    : AlgorithmResult.Fail(_owner);
            });
            return this;
        }

        public AlgorithmBody DoAlgorithm(IAlgorithm child, Func<Task<AlgorithmResult>> start)
        {
            Schedule(async () =>
            {
                _token.ThrowIfCancellationRequested();
                _owner.AddAlgorithm(child);
                return await start();
            });
            return this;
        }

        public AlgorithmBody Interlock(Func<IAsyncDisposable> interlock, Func<AlgorithmBody, AlgorithmBody> inner)
        {
            Schedule(async () =>
            {
                _token.ThrowIfCancellationRequested();
                await using var hold = interlock();
                var result = await inner(new AlgorithmBody(_owner, _token)).Start();
                return result.Ok ? AlgorithmResult.Success() : result;
            });
            return this;
        }

        public AlgorithmBody Subscribe(Action subscribe, Action unsubscribe)
        {
            Schedule(() =>
            {
                _token.ThrowIfCancellationRequested();
                subscribe();
                _subscriptions.Add(unsubscribe);
                return Task.FromResult(AlgorithmResult.Success());
            });
            return this;
        }

        public AlgorithmBody Unsubscribe(Action unsubscribe)
        {
            Schedule(() =>
            {
                unsubscribe();
                _subscriptions.Remove(unsubscribe);
                return Task.FromResult(AlgorithmResult.Success());
            });
            return this;
        }

        public async Task<AlgorithmResult> Start()
        {
            try
            {
                foreach (var step in _actions)
                {
                    _token.ThrowIfCancellationRequested();
                    var result = await step();
                    if (!result.Ok)
                        return result;
                }
                return AlgorithmResult.Success();
            }
            finally
            {
                foreach (var unsub in _subscriptions)
                    unsub();
                _subscriptions.Clear();
            }
        }
    }
}
