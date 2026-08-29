using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Oratoria.Domain.Connection.Pollers
{
    public abstract class Poller : IDisposable, INotifyPropertyChanged
    {
        protected readonly Stopwatch sw = new();

        private readonly IConnectionConfig _netConfig;
        private bool _isEnabled;

        public string PollerName { get; }

        public IConnectionConfig Connection => _netConfig;

        public bool IsConnected => _netConfig.IsConnected;

        public bool IsEnabled
        {
            get => _isEnabled;
            private set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        protected Poller(IConnectionConfig netConfig, string pollerName)
        {
            _netConfig = netConfig;
            PollerName = pollerName;
        }

        public void Enable() => IsEnabled = true;

        public void Disable() => IsEnabled = false;

        public virtual Task<bool> Connect()
        {
            return _netConfig.Connect();
        }

        public virtual void Disconnect()
        {
            _netConfig.CloseConnection();
        }

        public virtual void Dispose()
        {
            Disconnect();
        }

        public abstract void Survey();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
