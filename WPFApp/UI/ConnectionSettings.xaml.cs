using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using NLog;
using Oratoria36.Models;
using Oratoria36.Service;

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
        private readonly NetContext _moduleManager;

        public ModuleConfig Module1 { get; }
        public ModuleConfig Module2 { get; }

        public ICommand ApplySettingsCommandModule1 { get; }
        public ICommand ConnectCommandModule1 { get; }
        public ICommand DisconnectCommandModule1 { get; }

        public ICommand ApplySettingsCommandModule2 { get; }
        public ICommand ConnectCommandModule2 { get; }
        public ICommand DisconnectCommandModule2 { get; }

        #region Module1 Properties

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

        #endregion

        #region Module2 Properties

        private string _module2Status;
        public string Module2Status
        {
            get => _module2Status;
            set
            {
                _module2Status = value;
                OnPropertyChanged();
            }
        }

        private string _module2CurrentIP;
        public string Module2CurrentIP
        {
            get => _module2CurrentIP;
            set
            {
                _module2CurrentIP = value;
                OnPropertyChanged();
            }
        }

        private int _module2CurrentPort;
        public int Module2CurrentPort
        {
            get => _module2CurrentPort;
            set
            {
                _module2CurrentPort = value;
                OnPropertyChanged();
            }
        }

        private string _newIPModule2;
        public string NewIPModule2
        {
            get => _newIPModule2;
            set
            {
                _newIPModule2 = value;
                OnPropertyChanged();
            }
        }

        private string _newPortModule2;
        public string NewPortModule2
        {
            get => _newPortModule2;
            set
            {
                _newPortModule2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ConnectionSettingsVM()
        {
            _moduleManager = NetContext.Instance;
            Module1 = _moduleManager.Module1;
            Module2 = _moduleManager.Module2;

            ConnectCommandModule1 = new RelayCommand(ConnectModule1);
            DisconnectCommandModule1 = new RelayCommand(DisconnectModule1);
            ApplySettingsCommandModule1 = new RelayCommand(ApplySettingsModule1);

            ConnectCommandModule2 = new RelayCommand(ConnectModule2);
            DisconnectCommandModule2 = new RelayCommand(DisconnectModule2);
            ApplySettingsCommandModule2 = new RelayCommand(ApplySettingsModule2);

            UpdateModuleStatus();
        }

        private void UpdateModuleStatus()
        {
            Module1Status = Module1.IsConnected ? "Подключено" : "Отключено";
            Module1CurrentIP = Module1.IP;
            Module1CurrentPort = Module1.Port;

            Module2Status = Module2.IsConnected ? "Подключено" : "Отключено";
            Module2CurrentIP = Module2.IP;
            Module2CurrentPort = Module2.Port;
        }

        private async void ConnectModule1(object parameter)
        {
            _logger.Info($"Попытка подключения к {Module1.IP}:{Module1.Port}");
            await Module1.Connect(Module1.IP);
            UpdateModuleStatus();
        }

        private void DisconnectModule1(object parameter)
        {
            _logger.Info($"Отключение от {Module1.IP}:{Module1.Port}");
            Module1.CloseConnection();
            UpdateModuleStatus();
        }

        private async void ApplySettingsModule1(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewIPModule1) && string.IsNullOrWhiteSpace(NewPortModule1))
            {
                return;
            }

            _logger.Info($"Начало применения настроек. Новый IP модуля 1: '{NewIPModule1}', новый порт модуля 1: '{NewPortModule1}'");

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

            UpdateModuleStatus();

            NewIPModule1 = string.Empty;
            NewPortModule1 = string.Empty;
        }

        private async void ConnectModule2(object parameter)
        {
            _logger.Info($"Попытка подключения к {Module2.IP}:{Module2.Port}");
            await Module2.Connect(Module2.IP);
            UpdateModuleStatus();
        }

        private void DisconnectModule2(object parameter)
        {
            _logger.Info($"Отключение от {Module2.IP}:{Module2.Port}");
            Module2.CloseConnection();
            UpdateModuleStatus();
        }

        private async void ApplySettingsModule2(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewIPModule2) && string.IsNullOrWhiteSpace(NewPortModule2))
            {
                return;
            }

            _logger.Info($"Начало применения настроек. Новый IP модуля 2: '{NewIPModule2}', новый порт модуля 2: '{NewPortModule2}'");

            string ipToApply = string.IsNullOrWhiteSpace(NewIPModule2) ? Module2.IP : NewIPModule2;
            int portToApply = Module2.Port;

            if (!string.IsNullOrWhiteSpace(NewPortModule2) && int.TryParse(NewPortModule2, out int newPort))
            {
                portToApply = newPort;
            }
            else if (!string.IsNullOrWhiteSpace(NewPortModule2))
            {
                _logger.Error($"Не удалось преобразовать порт '{NewPortModule2}' в число");
                return;
            }

            if (ipToApply == Module2.IP && portToApply == Module2.Port)
            {
                _logger.Info("Настройки не изменились, сохранение не требуется");
                return;
            }

            _logger.Info($"Применение новых настроек: IP={ipToApply}, Port={portToApply}");

            Module2.IP = ipToApply;
            Module2.Port = portToApply;

            try
            {
                await _moduleManager.SaveConnectionSettingsAsync();
                _logger.Info("Настройки успешно применены");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при сохранении настроек");
            }

            UpdateModuleStatus();

            NewIPModule2 = string.Empty;
            NewPortModule2 = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
