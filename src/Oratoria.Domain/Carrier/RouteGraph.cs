using Microsoft.Extensions.Logging;
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
        public RouteGraph(
           RouteNode sourceNode,
           Carrier carrier,
           ILogger logger) : base(logger)
        {
            this.sourceNode = sourceNode;
            this.carrier = carrier;
        }

        public Task<AlgorithmResult> Transfer(RouteNode from, RouteNode to)
        {
            if (from == null)
                throw new ArgumentNullException("исходная точка не назначена");

            if (to == null)
                throw new ArgumentNullException("целевая точка не назначена");

            if (from.NodeId == sourceNode.NodeId || to.NodeId == sourceNode.NodeId)
                throw new InvalidOperationException("перенос невозможен");

            return Execute(() => carrier.CanCarry(from, to) && !sourceNode.IsEmpty,
                body => body
                .DoAlgorithm(carrier, () => carrier.Carry(from, sourceNode))
                .DoAlgorithm(carrier, () => carrier.Carry(sourceNode, to)));
        }
    }
}