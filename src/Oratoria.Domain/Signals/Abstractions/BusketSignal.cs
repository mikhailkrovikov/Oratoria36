namespace Oratoria.Domain.Signals.Abstractions
{
    public abstract class BusketSignal<T> : Signal <T>
    {
        public ushort PinNumber
        {
            get => field;
            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        }

        public BusketSignal(string name, ushort pinNumber) : base(name)
        {
            PinNumber = pinNumber;
        }
    }
}
