using Microsoft.Extensions.Logging;
using Oratoria.Domain.Algorithms;

namespace Oratoria.Domain.Carrier
{
    public class Carrier : AlgorithmBase
    {
        public Carrier(ILogger logger) : base(logger)
        {
        }

        public bool CanCarry(RouteNode from, RouteNode to)
        {
            if (from.NodeId == to.NodeId)
            {
                Reason = "начальная и конечная точки совпадают";
                return false;
            }

            if (from.IsEmpty)
            {
                Reason = "Исходная точка пуста";
                return false;
            }

            if (!to.IsEmpty)
            {
                Reason = "Целевая точка занята";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Элементарный перенос из точки в точку
        /// </summary>
        /// <param name="from">первая точка</param>
        /// <param name="to">вторая точка</param>
        /// <returns></returns>
        public Task<AlgorithmResult> Carry(RouteNode from, RouteNode to)
        {
            return Execute(() => CanCarry(from, to),
                body => body
                .DoAlgorithm(from.Unload.Owner, () => from.Unload.Run())
                .DoAlgorithm(to.Load.Owner, to.Load.Run)
                .DoTask(() =>
                {
                    from.IsEmpty = true;
                    to.IsEmpty = false;
                    return Task.FromResult(true);
                }));
        }
    }

    public class RouteOperation
    {
        public IAlgorithm Owner;
        public Func<Task<AlgorithmResult>> Run;
    }

    public class RouteNode
    {
        public string NodeId { get; }
        public bool IsEmpty { get; set; } = true;

        public RouteOperation Load { get; }
        public RouteOperation Unload { get; }

        public RouteNode(string NodeId, RouteOperation Load, RouteOperation Unload)
        {
            this.NodeId = NodeId;
            this.Load = Load;
            this.Unload = Unload;
        }
    }

    /// <summary>
    /// Граф для обхода 
    /// </summary>
    public class RouteGraph : AlgorithmBase
    {
        RouteNode sourceNode;
        Carrier carrier;
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