using Oratoria36.Models;
using Oratoria36.Service;
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
            DataContext = new ConnectionSettingsVM();
        }
    }
    public class ConnectionSettingsVM : INotifyPropertyChanged
    {
        ModuleManager _moduleManager;
        public ModuleConfig Module1 { get; }
        public ICommand ApplySettingsCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        private string _module1Status;
        public string Module1Status
        {
            get => _module1Status;
            set
            {
                _module1Status = value;
                OnPropertyChanged();
            }
        }

        private string _module1CurrentIP;
        public string Module1CurrentIP
        {
            get => _module1CurrentIP;
            set
            {
                _module1CurrentIP = value;
                OnPropertyChanged();
            }
        }

        private int _module1CurrentPort;
        public int Module1CurrentPort
        {
            get => _module1CurrentPort;
            set
            {
                _module1CurrentPort = value;
                OnPropertyChanged();
            }
        }

        private string _newIP;
        public string NewIP
        {
            get => _newIP;
            set
            {
                _newIP = value;
                OnPropertyChanged();
            }
        }

        private string _newPort;
        public string NewPort
        {
            get => _newPort;
            set
            {
                _newPort = value;
                OnPropertyChanged();
            }
        }

        private async void Connect(object parameter)
        {
            await Module1.InitializeModbusAsync(Module1.IP);
            UpdateModule1Status();
        }
        private void UpdateModule1Status()
        {
            Module1Status = Module1.IsConnected ? "Подключено" : "Отключено";
        }
        private void Disconnect(object parameter)
        {
            Module1.CloseConnection();
            UpdateModule1Status();
        }
        public ConnectionSettingsVM()
        {
            _moduleManager = new ModuleManager();

            Module1 = _moduleManager.Module1;            
            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
        }      

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}