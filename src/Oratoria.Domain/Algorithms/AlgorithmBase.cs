using Microsoft.Extensions.Logging;

namespace Oratoria.Domain.Algorithms
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        private CancellationTokenSource _ctSource = new();

        protected AlgorithmBase(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected CancellationToken Token => _ctSource.Token;

        public AlgorithmStatus Status
        {
            get => field;
            protected set
            {
                if (value != field)
                {
                    field = value;
                    StateChanged?.Invoke();
                }
            }
        }

        public List<IAlgorithm> Children { get; set; } = new();

        public string? Reason { get; set; }


        public event Action? StateChanged;

        public void Cancel()
        {
            try
            {
                _ctSource.Cancel();
                foreach (var child in Children)
                    child.Cancel();
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        protected void ResetToken()
        {
            try
            {
                _ctSource.Cancel();
                _ctSource.Dispose();
            }
            catch { }
            _ctSource = new CancellationTokenSource();
        }

        public void AddAlgorithm(IAlgorithm algorithm)
        {
            Children.Add(algorithm);
        }

        public async Task<AlgorithmResult> Execute(Func<bool> canExecute, Func<AlgorithmBody, AlgorithmBody> build, CancellationToken parentToken = default)
        {
            if (Status == AlgorithmStatus.Running)
            {
                Reason = "уже выполняется";
                return AlgorithmResult.Fail(this);
            }

            if (!canExecute())
                return AlgorithmResult.Blocked(this);

            ResetToken();
            Children.Clear();
            Reason = string.Empty;
            Status = AlgorithmStatus.Running;

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_ctSource.Token, parentToken);
            try
            {
                var result = await build(new AlgorithmBody(this, linked.Token)).Start();
                if (result.Ok)
                {
                    Status = AlgorithmStatus.Completed;
                }
                else
                {
                    Status = result.Status;
                }
                return result;
            }
            catch (OperationCanceledException ex)
            {
                Status = AlgorithmStatus.Cancelled;
                Logger.LogWarning("отмена операции");
                return AlgorithmResult.Canceled(this);
            }
            catch (Exception ex)
            {
                Status = AlgorithmStatus.Failed;
                Reason = ex.Message;
                Logger.LogError(ex.Message);
                return AlgorithmResult.Fail(this);
            }
        }
    }
}
