using NLog;
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
            DataContext = LogViewModel.Instance;
        }
    }
    public class LogViewModel : INotifyPropertyChanged
    {
        private static LogViewModel _instance;
        public static LogViewModel Instance => _instance ??= new LogViewModel();

        private LogViewModel()
        {
            var target = LogManager.Configuration.FindTargetByName<ObservableCollectionTarget>("observableCollection");
            Logs = target?.Logs ?? new ObservableCollection<string>();
        }

        public ObservableCollection<string> Logs { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}