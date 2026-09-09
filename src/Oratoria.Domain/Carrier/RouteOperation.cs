using Oratoria.Domain.Algorithms;

namespace Oratoria.Domain.Carrier
{
    public class RouteOperation
    {
        public IAlgorithm Owner;
        public Func<Task<AlgorithmResult>> Run;
    }
}