using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Oratoria36.Models;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = _vm; // Изменено с this на _vm

            _vm.StartClock();
            NavigationBarControl.PageChanged += NavigateToPage;
            MainFrame.NavigationService.Navigate(new MainPage());

            // Исправлена подписка на событие ConnectionStatusChanged
            ModuleManager.Instance.ConnectionStatusChanged += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateConnectionStatus();
                });
            };

            // Запускаем подключение ко всем модулям
            _ = ModuleManager.Instance.ConnectAllAsync();
        }

        // Метод для обновления статуса подключения
        private void UpdateConnectionStatus()
        {
            var manager = ModuleManager.Instance;
            bool anyConnected = manager.Module1.IsConnected ||
                               manager.Module2.IsConnected ||
                               manager.Module3.IsConnected ||
                               manager.Module4.IsConnected ||
                               manager.TransportModule.IsConnected;

            _vm.ConnectionStatus = anyConnected ? "Подключено" : "Не подключено";
        }

        private void NavigateToPage(string pageName)
        {
            switch (pageName)
            {
                case "MainPage":
                    MainFrame.Navigate(new MainPage());
                    break;
                case "SignalsPage":
                    MainFrame.Navigate(new SignalsPage());
                    break;
                case "ConnectionSettings":
                    MainFrame.Navigate(new ConnectionSettings());
                    break;
                case "LogPage":
                    MainFrame.Navigate(new LogPage());
                    break;
                default:
                    throw new ArgumentException($"Unknown page: {pageName}");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NavigationBarControl.PageChanged -= NavigateToPage;
            ModuleManager.Instance.DisconnectAll();
        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private string _date;
        private string _time;
        private string _connectionStatus = "Не подключено";

        public string Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                if (_connectionStatus != value)
                {
                    _connectionStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public void StartClock()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += UpdateDateTime;
            _timer.Start();
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToString("D");
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
