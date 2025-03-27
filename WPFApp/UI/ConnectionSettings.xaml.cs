using Oratoria36.Models;
using Oratoria36.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using NLog;

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
        private static readonly Logger _logger = LogManager.GetLogger("Settings");

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
            _logger.Info($"Попытка подключения к {Module1.IP}:{Module1.Port}");
            await Module1.InitializeModbusAsync(Module1.IP);
            UpdateModule1Status();
        }

        private void UpdateModule1Status()
        {
            Module1Status = Module1.IsConnected ? "Подключено" : "Отключено";
            Module1CurrentIP = Module1.IP;
            Module1CurrentPort = Module1.Port;
        }

        private void Disconnect(object parameter)
        {
            _logger.Info($"Отключение от {Module1.IP}:{Module1.Port}");
            Module1.CloseConnection();
            UpdateModule1Status();
        }

        private async void ApplySettings(object parameter)
        {

            if (string.IsNullOrWhiteSpace(NewIP) && string.IsNullOrWhiteSpace(NewPort))
            {

                return;
            }

            _logger.Info($"Начало применения настроек. NewIP: '{NewIP}', NewPort: '{NewPort}'");

            string ipToApply = string.IsNullOrWhiteSpace(NewIP) ? Module1.IP : NewIP;

            int portToApply = Module1.Port;
            if (!string.IsNullOrWhiteSpace(NewPort) && int.TryParse(NewPort, out int newPort))
            {
                portToApply = newPort;
            }
            else if (!string.IsNullOrWhiteSpace(NewPort))
            {
                _logger.Error($"Не удалось преобразовать порт '{NewPort}' в число");
                return;
            }

            if (ipToApply == Module1.IP && portToApply == Module1.Port)
            {
                _logger.Info("Настройки не изменились, сохранение не требуется");
                return;
            }

            _logger.Info($"Применение новых настроек: IP={ipToApply}, Port={portToApply}");

            Module1.IP = ipToApply;
            Module1.Port = portToApply;

            try
            {
                await _moduleManager.SaveConnectionSettingsAsync();
                _logger.Info("Настройки успешно применены");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при сохранении настроек");
            }

            UpdateModule1Status();

            NewIP = string.Empty;
            NewPort = string.Empty;
        }

        public ConnectionSettingsVM()
        {

                _moduleManager = ModuleManager.Instance; 
                Module1 = _moduleManager.Module1;

                ConnectCommand = new RelayCommand(Connect);
                DisconnectCommand = new RelayCommand(Disconnect);
                ApplySettingsCommand = new RelayCommand(ApplySettings);

                UpdateModule1Status();



        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}