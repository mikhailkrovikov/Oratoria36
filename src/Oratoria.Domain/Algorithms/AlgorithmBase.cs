namespace Oratoria.Domain.Algorithms
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        private CancellationTokenSource? ctsource;
        private readonly object locker = new();

        public AlgorithmStatus Status { get; protected set; }

        public string? Reason { get; protected set; }

        public void Cancel()
        {
            CancellationTokenSource? source;

            lock (locker)
            {
                source = ctsource;
            }

            try
            {
                source?.Cancel();
            }
            catch
            {
            }
        }

        public async Task<AlgorithmResult> Execute(
            Func<bool> canExecute,
            Func<AlgorithmBody, AlgorithmBody> build,
            CancellationToken parentToken = default)
        {
            CancellationTokenSource ctSource;

            lock (locker)
            {
                if (ctsource != null)
                    return AlgorithmResult.Blocked(this);

                ctSource = CancellationTokenSource
                    .CreateLinkedTokenSource(parentToken);

                ctsource = ctSource;
            }

            try
            {
                Reason = string.Empty;
                ctSource.Token.ThrowIfCancellationRequested();

                if (!canExecute())
                {
                    Status = AlgorithmStatus.Blocked;
                    return AlgorithmResult.Blocked(this);
                }

                Status = AlgorithmStatus.Running;

                var result = await build(
                    new AlgorithmBody(ctSource.Token)).ExecuteAsync();

                ctSource.Token.ThrowIfCancellationRequested();

                result = result.WithSource(this);

                if (!result.Ok && string.IsNullOrEmpty(Reason))
                    Reason = result.Failed?.Reason;

                Status = result.Status;
                return result;
            }
            catch (OperationCanceledException)
            {
                Status = AlgorithmStatus.Canceled;
                return AlgorithmResult.Canceled(this);
            }
            catch (Exception ex)
            {
                Reason = ex.Message;
                Status = AlgorithmStatus.Failed;
                return AlgorithmResult.Fail(this);
            }
            finally
            {
                lock (locker)
                {
                    ctsource = null;
                    ctSource.Dispose();
                }
            }
        }
    }
}
