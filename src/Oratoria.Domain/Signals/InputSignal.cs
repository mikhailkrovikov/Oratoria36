using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Signals
{
    public class InputSignal<T> : BusketSignal<T>
    {

        public override event SignalChangedHandler OnSignalChanged;

        IInputStrategy<T> _strategy;

        private T _value;
        public override T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    RaiseEventParallel(_value);
                    OnPropertyChanged();
                }
            }
        }

        public InputSignal(string name, ushort pinNumber, IInputStrategy<T> strategy) : base(name, pinNumber)
        {
            _strategy = strategy;
        }

        public override bool GetNewValue()
        {
            try
            {
                Value = _strategy.GetInput(PinNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void RaiseEventParallel(T value)
        {
            var handlers = OnSignalChanged?.GetInvocationList();
            if (handlers == null) return;

            var tasks = new List<Task>();

            foreach (SignalChangedHandler handler in handlers)
            {
                tasks.Add(Task.Run(() => handler.Invoke(value)));
            }

            Task.WaitAll(tasks.ToArray());
        }
        public override void ResetSignal()
        {
            Value = default;
        }
    }
}
