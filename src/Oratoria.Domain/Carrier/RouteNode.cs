using Oratoria.Domain.Algorithms;

namespace Oratoria.Domain.Carrier
{
    public class RouteNode
    {
        public string NodeId { get; }

        public bool IsEmpty { get; set; } = true;

        public Func<CancellationToken, Task<AlgorithmResult>> Load { get; }

        public Func<CancellationToken, Task<AlgorithmResult>> Unload { get; }

        public RouteNode(
            string nodeId,
            Func<CancellationToken, Task<AlgorithmResult>> load,
            Func<CancellationToken, Task<AlgorithmResult>> unload)
        {
            NodeId = nodeId;
            Load = load;
            Unload = unload;
        }
    }
}