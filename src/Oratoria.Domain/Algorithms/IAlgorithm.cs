namespace Oratoria.Domain.Algorithms
{
    public interface IAlgorithm
    {
        AlgorithmStatus Status { get; }

        List<IAlgorithm> Children { get; }

        string? Reason { get; }

        event Action? StateChanged;

        void Cancel();
    }
}
