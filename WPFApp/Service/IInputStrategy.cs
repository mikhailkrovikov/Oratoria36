namespace Oratoria36.Models.Signals
{
    public interface IInputStrategy<T>
    {
        public event Action<T> OnSignalChanged;
        public string Name { get; set; }
        public ushort PinNumber { get; set; }
        public T GetInput(ushort pinNumber);
    }

    public interface IOutputStrategy<T>
    {
        public void SetOutput(ushort pinNumber, T value);
    }
}
