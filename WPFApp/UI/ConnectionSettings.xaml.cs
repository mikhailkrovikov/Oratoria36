﻿using Oratoria36.Models;
using Oratoria36.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

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
            Module1CurrentIP = Module1.IP;
            Module1CurrentPort = Module1.Port;
        }

        private void Disconnect(object parameter)
        {
            Module1.CloseConnection();
            UpdateModule1Status();
        }

        private async void ApplySettings(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewIP) || string.IsNullOrWhiteSpace(NewPort))
                return;

            if (int.TryParse(NewPort, out int port))
            {
                Module1.IP = NewIP;
                Module1.Port = port;

                // Сохраняем настройки в JSON
                await _moduleManager.SaveConnectionSettingsAsync();

                UpdateModule1Status();

                // Очищаем поля ввода
                NewIP = string.Empty;
                NewPort = string.Empty;
            }
        }

        public ConnectionSettingsVM()
        {
            _moduleManager = new ModuleManager();

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

    public class RelayCommand : ICommand
    {
        private readonly System.Action<object> _execute;
        private readonly System.Func<object, bool> _canExecute;

        public RelayCommand(System.Action<object> execute, System.Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}