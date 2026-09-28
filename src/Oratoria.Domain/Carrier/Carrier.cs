using Oratoria.Domain.Algorithms;

namespace Oratoria.Domain.Carrier
{
    public class Carrier : AlgorithmBase
    {
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

        public Task<AlgorithmResult> Carry(RouteNode from, RouteNode to, CancellationToken token = default)
        {
            return Execute(() => CanCarry(from, to),
                body => body
                .DoAlgorithm(from.Unload)
                .DoAlgorithm(to.Load)
                .DoAction(() =>
                {
                    from.IsEmpty = true;
                    to.IsEmpty = false;
                }),
                token);
        }
    }
}
