namespace Oratoria.Domain.Algorithms
{
    public sealed class AlgorithmBody
    {
        private readonly CancellationToken _token;
        private readonly List<Func<Task<AlgorithmResult>>> _actions = new();
        private readonly List<Action> _subscriptions = new();

        public AlgorithmBody(CancellationToken token)
        {
            _token = token;
        }

        /// <summary>
        /// Использовать для методов расширения AlgorithmBody
        /// </summary>
        /// <param name="func"></param>
        public void Schedule(Func<Task<AlgorithmResult>> func)
        {
            _actions.Add(func);
        }

        /// <summary>
        /// Выполняет вложенный алгоритм.
        /// Передаёт ему отмену и сохраняет источник его ошибки.
        /// </summary>
        public AlgorithmBody DoAlgorithm(
            Func<CancellationToken, Task<AlgorithmResult>> action)
        {
            Schedule(() => action(_token));
            return this;
        }

        /// <summary>
        /// Выполняет действие устройства.
        /// При false завершает алгоритм с ошибкой.
        /// Передаёт действию токен отмены алгоритма.
        /// </summary>
        public AlgorithmBody DoTask(
            Func<CancellationToken, Task<bool>> action)
        {
            Schedule(async () =>
            {
                var ok = await action(_token);
                if (ok)
                {
                    return AlgorithmResult.Success();
                }
                else
                {
                    return AlgorithmResult.Fail();
                }
            });
            return this;
        }

        /// <summary>
        /// Выполняет короткое синхронное действие.
        /// Исключение завершает алгоритм с ошибкой.
        /// </summary>
        public AlgorithmBody DoAction(Action action)
        {
            Schedule(() =>
            {
                action();
                return Task.FromResult(AlgorithmResult.Success());
            });
            return this;
        }

        /// <summary>
        /// Параллельное выполнение списка действий
        /// </summary>
        /// <param name="actions"></param>
        /// <returns></returns>
        public AlgorithmBody Parallel(
            params Func<CancellationToken, Task<bool>>[] actions)
        {
            Schedule(async () =>
            {
                var results = await Task.WhenAll(
                    actions.Select(action => action(_token)));

                if (results.All(ok => ok))
                {
                    return AlgorithmResult.Success();
                }
                else
                {
                    return AlgorithmResult.Fail();
                }
            });
            return this;
        }

        /// <summary>
        /// Подписка на событие внутри алгоритма
        /// </summary>
        /// <param name="subscribe">подписка на событие</param>
        /// <param name="unsubscribe">отпика от этого же события для исключения замыкания</param>
        /// <returns></returns>
        public AlgorithmBody Subscribe(
            Action subscribe,
            Action unsubscribe)
        {
            Schedule(() =>
            {
                subscribe();
                _subscriptions.Add(unsubscribe);
                return Task.FromResult(AlgorithmResult.Success());
            });
            return this;
        }

        /// <summary>
        /// Отписка от события
        /// </summary>
        /// <param name="unsubscribe"></param>
        /// <returns></returns>
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

        internal async Task<AlgorithmResult> ExecuteAsync()
        {
            try
            {
                _token.ThrowIfCancellationRequested();

                foreach (var step in _actions)
                {
                    _token.ThrowIfCancellationRequested();

                    var result = await step();

                    _token.ThrowIfCancellationRequested();

                    if (!result.Ok)
                        return result;
                }

                return AlgorithmResult.Success();
            }
            finally
            {
                foreach (var unsubscribe in _subscriptions
                    .AsEnumerable()
                    .Reverse()
                    .ToArray())
                {
                    try
                    {
                        unsubscribe();
                    }
                    catch
                    {
                    }
                }
                _subscriptions.Clear();
            }
        }
    }
}
