using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oratoria36.Models.Signals
{
    public abstract class Signal : INotifyPropertyChanged
    {
        public string Name { get; }
        public ushort Channel { get; }

        private bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        protected Signal(string name, ushort channel)
        {
            Name = name;
            Channel = channel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
