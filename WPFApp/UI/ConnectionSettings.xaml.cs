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
        private static readonly Logger _logger = LogManager.GetLogger("Настройки");

        ModuleManager _moduleManager;
        public ModuleConfig Module1 { get; }
        public ICommand ApplySettingsCommandModule1 { get; }
        public ICommand ConnectCommandModule1 { get; }
        public ICommand DisconnectCommandModule1 { get; }

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

        private string _newIPModule1;
        public string NewIPModule1
        {
            get => _newIPModule1;
            set
            {
                _newIPModule1 = value;
                OnPropertyChanged();
            }
        }

        private string _newPortModule1;
        public string NewPortModule1
        {
            get => _newPortModule1;
            set
            {
                _newPortModule1 = value;
                OnPropertyChanged();
            }
        }

        private async void ConnectModule1(object parameter)
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

        private void DisconnectModule1(object parameter)
        {
            _logger.Info($"Отключение от {Module1.IP}:{Module1.Port}");
            Module1.CloseConnection();
            UpdateModule1Status();
        }

        private async void ApplySettingsModule1(object parameter)
        {

            if (string.IsNullOrWhiteSpace(NewIPModule1) && string.IsNullOrWhiteSpace(NewPortModule1))
            {

                return;
            }

            _logger.Info($"Начало применения настроек. NewIPModule1: '{NewIPModule1}', NewPortModule1: '{NewPortModule1}'");

            string ipToApply = string.IsNullOrWhiteSpace(NewIPModule1) ? Module1.IP : NewIPModule1;

            int portToApply = Module1.Port;
            if (!string.IsNullOrWhiteSpace(NewPortModule1) && int.TryParse(NewPortModule1, out int newPort))
            {
                portToApply = newPort;
            }
            else if (!string.IsNullOrWhiteSpace(NewPortModule1))
            {
                _logger.Error($"Не удалось преобразовать порт '{NewPortModule1}' в число");
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

            NewIPModule1 = string.Empty;
            NewPortModule1 = string.Empty;
        }

        public ConnectionSettingsVM()
        {

                _moduleManager = ModuleManager.Instance; 
                Module1 = _moduleManager.Module1;

                ConnectCommandModule1 = new RelayCommand(ConnectModule1);
                DisconnectCommandModule1 = new RelayCommand(DisconnectModule1);
                ApplySettingsCommandModule1 = new RelayCommand(ApplySettingsModule1);

                UpdateModule1Status();



        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}