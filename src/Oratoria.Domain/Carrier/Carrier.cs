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
}