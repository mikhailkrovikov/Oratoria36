using Oratoria.Domain.Algorithms;

namespace Oratoria.Domain.Carrier
{
    /// <summary>
    /// Граф для обхода 
    /// </summary>
    public class RouteGraph : AlgorithmBase
    {
        private readonly RouteNode sourceNode;
        private readonly Carrier carrier;
        public RouteGraph(RouteNode sourceNode, Carrier carrier)
        {
            this.sourceNode = sourceNode;
            this.carrier = carrier;
        }

        public Task<AlgorithmResult> Transfer(RouteNode from, RouteNode to, CancellationToken token = default)
        {
            if (from == null)
            {
                throw new ArgumentNullException("исходная точка не назначена");
            }

            if (to == null)
            {
                throw new ArgumentNullException("целевая точка не назначена");
            }

            if (from.NodeId == sourceNode.NodeId || to.NodeId == sourceNode.NodeId)
            {
                throw new InvalidOperationException($"{sourceNode} не может быть точкой отправления назначения");
            }

            return Execute(
                () => carrier.CanCarry(from, to) && sourceNode.IsEmpty,
                body => body
                    .DoAlgorithm(ct => carrier.Carry(from, sourceNode, ct))
                    .DoAlgorithm(ct => carrier.Carry(sourceNode, to, ct)),
                token);
        }
    }
}