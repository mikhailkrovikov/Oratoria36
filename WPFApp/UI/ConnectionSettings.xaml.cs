using Oratoria36.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI
{
    public partial class ConnectionSettings : Page
    {
        public ConnectionSettings()
        {
            InitializeComponent();
            DataContext = new ConnectionSettingsViewModel();
        }
    }
    public class ConnectionSettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ModuleConfigViewModel> Modules { get; }

        public ConnectionSettingsViewModel()
        {
            Modules = new ObservableCollection<ModuleConfigViewModel>
            {
                new ModuleConfigViewModel(ModulesConfigManager.GetDevice("Module1")),
                new ModuleConfigViewModel(ModulesConfigManager.GetDevice("Module2")),
                new ModuleConfigViewModel(ModulesConfigManager.GetDevice("Module3")),
                new ModuleConfigViewModel(ModulesConfigManager.GetDevice("Module4")),
                new ModuleConfigViewModel(ModulesConfigManager.GetDevice("Module5"))
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ModuleConfigViewModel : INotifyPropertyChanged
    {
        private readonly ModuleConfig _moduleConfig;
        private string _newIp;

        public ModuleConfigViewModel(ModuleConfig moduleConfig)
        {
            _moduleConfig = moduleConfig;
            ApplyNewIPCommand = new RelayCommand(ApplyNewIP);
        }

        public string IP => _moduleConfig.IP;

        public string NewIP
        {
            get => _newIp;
            set
            {
                _newIp = value;
                OnPropertyChanged();
            }
        }

        public ICommand ApplyNewIPCommand { get; }

        private async void ApplyNewIP()
        {
            if (!string.IsNullOrWhiteSpace(_newIp))
            {
                _moduleConfig.SetIP(_newIp);
                await _moduleConfig.InitializeModbusAsync(_newIp);
                OnPropertyChanged(nameof(IP));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}