namespace Oratoria.Domain.Signals.Abstractions
{
    public interface IInputStrategy<T>
    {
        public T GetInput(ushort pinNumber);
    }
}
