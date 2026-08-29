using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class DITestStrategy : IInputStrategy<bool>
    {
        public bool GetInput(ushort pinNumber)
        {
            return default;
        }
    }
}
