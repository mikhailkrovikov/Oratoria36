using Oratoria.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Oratoria.UI.ViewModels;

public class MainWindowVM : INotifyPropertyChanged
{
    public ObservableCollection<object> Logs { get; } = new();
    public ObservableCollection<object> Alarms { get; } = new();

    public ICommand CloseButtonCommand { get; } = new RelayCommand(_ =>
    {
        var result = MessageBox.Show(
            "Вы уверены, что хотите выйти из программы?",
            "Подтверждение выхода",
            MessageBoxButton.OKCancel,
            MessageBoxImage.Question);
        if (result == MessageBoxResult.OK)
            Application.Current.Shutdown();
    });

    public ICommand AutorizationCommand { get; } = new RelayCommand(_ => { });

    public ICommand ServiceModeCommand { get; } = new RelayCommand(_ => { });

    public string ServiceModeButtonText => "Сервисный режим";

    private DispatcherTimer? _timer;
    private string _date = string.Empty;
    private string _time = string.Empty;

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
        UpdateDateTime(null, EventArgs.Empty);
    }

    private void UpdateDateTime(object? sender, EventArgs e)
    {
        Date = DateTime.Now.ToString("dd.MM.yyyy");
        Time = DateTime.Now.ToString("HH:mm:ss");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
