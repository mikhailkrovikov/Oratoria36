namespace Oratoria.Domain.Algorithms
{
    public sealed class AlgorithmResult
    {
        public readonly bool Ok;
        public readonly AlgorithmStatus Status;
        public readonly IAlgorithm? Failed;

        private AlgorithmResult(bool ok, AlgorithmStatus status, IAlgorithm? failed)
        { 
            Ok = ok; 
            Status = status;
            Failed = failed; 
        }

        public static AlgorithmResult Success()
        {
            return new AlgorithmResult(true, AlgorithmStatus.Completed, null);
        }

        public static AlgorithmResult Fail(IAlgorithm failed)
        {
            return new AlgorithmResult(false, AlgorithmStatus.Failed, failed);
        }

        public static AlgorithmResult Canceled(IAlgorithm failed)
        {
            return new AlgorithmResult(false, AlgorithmStatus.Cancelled, failed);
        }

        public static AlgorithmResult Blocked(IAlgorithm failed)
        {
            return new AlgorithmResult(false, AlgorithmStatus.Blocked, failed);
        }
    }
}
