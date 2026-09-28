namespace Oratoria.Domain.Algorithms
{
    public interface IAlgorithm
    {
        AlgorithmStatus Status { get; }

        string? Reason { get; }

        void Cancel();
    }
}
