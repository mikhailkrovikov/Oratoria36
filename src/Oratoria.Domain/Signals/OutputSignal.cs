using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals
{
    public class OutputSignal<T> : BusketSignal<T>
    {
        private readonly IOutputStrategy<T> _strategy;
        
        public override event Action<T>? OnSignalChanged;

        T _value;
        public override T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    SetOutput(PinNumber, _value);
                    RaiseEventParallel(_value);
                    OnPropertyChanged();
                }
            }
        }

        public OutputSignal(string name, ushort pinNumber, IOutputStrategy<T> strategy) : base(name, pinNumber)
        {
            Name = name;
            PinNumber = pinNumber;
            _strategy = strategy;
        }
        public void SetOutput(ushort pinNumber, T value)
        {
            _strategy.SetOutput(pinNumber, value);
        }

        public void RaiseEventParallel(T value)
        {
            var handlers = OnSignalChanged?.GetInvocationList();
            if (handlers == null) return;

            var tasks = new List<Task>();

            foreach (Action<T> handler in handlers.Cast<Action<T>>())
                tasks.Add(Task.Run(() => handler.Invoke(value)));
            
        }

        public override void ResetSignal()
        {
            Value = default;
        }
    }
}
