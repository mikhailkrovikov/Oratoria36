using NLog;
using Oratoria36.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Oratoria36.UI
{
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();
            DataContext = new LogPageVM();
        }
    }
    public class LogPageVM : INotifyPropertyChanged
    {
        private ObservableCollection<LogEntry> _logs;

        public ObservableCollection<LogEntry> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged();
            }
        }

        public LogPageVM()
        {
            Logs = DataGridTarget.LogEntries;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}