namespace Oratoria.Domain.Algorithms
{
    public sealed class AlgorithmResult
    {
        public bool Ok
        {
            get
            {
                return Status == AlgorithmStatus.Completed;
            }
        }

        public AlgorithmStatus Status { get; }

        public IAlgorithm? Failed { get; }

        private AlgorithmResult(AlgorithmStatus status, IAlgorithm? failed = null)
        { 
            Status = status;
            Failed = failed; 
        }

        public static AlgorithmResult Success()
        {
            return new AlgorithmResult(AlgorithmStatus.Completed);
        }

        public static AlgorithmResult Fail(IAlgorithm? failed = null)
        {
            return new AlgorithmResult(AlgorithmStatus.Failed, failed);
        }

        public static AlgorithmResult Canceled(IAlgorithm? failed = null)
        {
            return new AlgorithmResult(AlgorithmStatus.Canceled, failed);
        }

        public static AlgorithmResult Blocked(IAlgorithm? failed = null)
        {
            return new AlgorithmResult(AlgorithmStatus.Blocked, failed);
        }

        public AlgorithmResult WithSource(IAlgorithm algorithm)
        {
            if (Ok || Failed != null)
                return this;

            return new AlgorithmResult(Status, algorithm);
        }

        public override string ToString()
        {
            return Status.ToString();
        }
    }
}
