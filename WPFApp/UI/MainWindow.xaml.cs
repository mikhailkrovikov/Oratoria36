using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using NLog;
using Oratoria36.Models;
using Oratoria36.Service;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetLogger("MainWindow");
        private readonly MainWindowVM _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = this; // Важно для привязки {Binding ViewModel.Date}

            _vm.StartClock();
            NavigationBarControl.PageChanged += NavigateToPage;
            MainFrame.NavigationService.Navigate(new MainPage());

            // Запускаем подключение ко всем модулям
            _ = ModuleManager.Instance.ConnectAllAsync();

            // Запускаем опрос сигналов
            ModbusPoller.Instance.StartPolling();

            _logger.Info("MainWindow инициализировано");
        }

        // Свойство для доступа к ViewModel из XAML
        public MainWindowVM ViewModel => _vm;

        private void NavigateToPage(string pageName)
        {
            _logger.Info($"Навигация на страницу: {pageName}");

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
                    _logger.Warn($"Неизвестная страница: {pageName}");
                    throw new ArgumentException($"Unknown page: {pageName}");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _logger.Info("Закрытие главного окна");

            NavigationBarControl.PageChanged -= NavigateToPage;

            // Останавливаем таймер
            _vm.StopClock();

            // Останавливаем опрос сигналов
            ModbusPoller.Instance.StopPolling();

            // Отключаем все модули
            ModuleManager.Instance.DisconnectAll();
        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetLogger("MainWindowVM");
        private DispatcherTimer _timer;
        private string _date;
        private string _time;

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

        public void StartClock()
        {
            _logger.Info("Запуск таймера времени");
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += UpdateDateTime;
            _timer.Start();

            // Инициализируем время сразу
            UpdateDateTime(null, null);
        }

        public void StopClock()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= UpdateDateTime;
                _logger.Info("Таймер времени остановлен");
            }
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToString("D");
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
