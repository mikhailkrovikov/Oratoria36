namespace Oratoria.Domain.Signals.Abstractions
{
    public abstract class BusketSignal<T> : Signal <T>
    {
        private ushort _pinNumber;
        
        public ushort PinNumber
        {
            get => _pinNumber;
            set
            {
                if (value != _pinNumber)
                {
                    _pinNumber = value;
                    OnPropertyChanged("PinNumber");
                }
            }
        }

        public BusketSignal(string name, ushort pinNumber) : base(name)
        {
            PinNumber = pinNumber;
        }
    }
}
