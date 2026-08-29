namespace Oratoria.Domain.Signals.Abstractions
{
    public interface IOutputStrategy<T>
    {
        public void SetOutput(ushort pinNumber, T value);
    }
}
