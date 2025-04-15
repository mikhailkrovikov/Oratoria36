using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using NLog;
using Oratoria36.Models;
using Oratoria36.Service;
using Oratoria36.UI.Signals;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;
        MainContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = this;

            _vm.StartClock();
            NavigationBarControl.PageChanged += NavigateToPage;
            MainFrame.NavigationService.Navigate(new MainPage());
            _context = MainContext.Instance;
        }

        public MainWindowVM ViewModel => _vm;

        private void NavigateToPage(string pageName)
        {
            switch (pageName)
            {
                case "MainPage":
                    MainFrame.Navigate(new MainPage());
                    break;
                case "Module2SignalsPage":
                    MainFrame.Navigate(new Module2SignalsPage());
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
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
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
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += UpdateDateTime;
            _timer.Start();
            UpdateDateTime(null, null);
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
