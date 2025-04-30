namespace Oratoria36.Models.Signals
{
    public interface IInputStrategy<T>
    {
        public T GetInput(ushort pinNumber);
    }

    public interface IOutputStrategy<T>
    {
        public void SetOutput(ushort pinNumber, T value);
    }

    public interface IInputSignal<T>
    {
        public T GetInput(ushort pinNumber);
    }

    public interface IOutputSignal<T>
    {
        public void SetOutput(ushort pinNumber, T value);
    }
}
