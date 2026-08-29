using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals.Strategies
{
    public class DOTestStrategy : IOutputStrategy<bool>
    {
        public void SetOutput(ushort pinNumber, bool value)
        {

        }
    }
}
