using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class AITestStrategy : IInputStrategy<double>
    {
        public double GetInput(ushort pinNumber)
        {
            return default;
        }
    }
}
