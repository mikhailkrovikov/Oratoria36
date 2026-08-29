using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Oratoria.Domain.Signals.Abstractions
{
    public abstract class Signal : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public abstract void ResetSignal();


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public abstract class Signal<T> : Signal
    {
        public delegate void SignalChangedHandler(T value);

        public virtual event SignalChangedHandler OnSignalChanged;
        [JsonIgnore]
        public virtual T Value { get; set; }

        public Signal(string name)
        {
            Name = name;
        }
        public virtual bool GetNewValue()
        {
            return default;
        }

    }
}
