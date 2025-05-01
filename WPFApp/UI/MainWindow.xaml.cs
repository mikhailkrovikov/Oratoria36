using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NLog;
using Oratoria36.Models;
using Oratoria36.Models.Connection;
using Oratoria36.Service;
using Oratoria36.UI.ModulePages.Module2;
using Oratoria36.UI.Signals;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;
        private readonly MainContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = _vm;
            _vm.StartClock();
            NavigationBarControl.HostFrame = MainFrame;
            _context = MainContext.Instance;
            MainFrame.Navigate(new MainPage());
        }

        public MainWindowVM ViewModel => _vm;
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        public ObservableCollection<LogEntry> Logs => DataGridTarget.LogEntries;
        public MainWindowVM()
        {
            CloseButtonCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }
        public ICommand CloseButtonCommand { get; }

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
