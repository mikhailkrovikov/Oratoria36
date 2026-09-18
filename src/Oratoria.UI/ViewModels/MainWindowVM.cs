using Oratoria.UI.Controls.Controls.Navigation;
using Oratoria.UI.Logging;
using Oratoria.UI.Services;
using Oratoria.UI.Controls.DialogWindows;
using Oratoria.UI.Views.Pages;
using Oratoria.UI.Views.Pages.Module2Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace Oratoria.UI.ViewModels;

public class MainWindowVM : INotifyPropertyChanged
{
    public MainWindowVM()
    {
        Navigation = new NavigationBuilder()
        .Item<MainPage>("Главная")
        .Group("Транспорт", transport => transport
            .Item<TransportSignalsPage>("Сигналы"))
        .Group("Вакуумная система", vacuum => vacuum
            .Item<VacuumSignalsPage>("Сигналы"))
        .Group("Модуль 2", m2 => m2
            .Item<Module2MnemoPage>("Мнемосхема")
            .Item<Module2RecipePage>("Рецепт")
            .Item<Module2SignalsPage>("Сигналы")
            .Item<Module2SettingsPage>("Настройки")
            .Item<Module2LogsPage>("Журнал"))
        .Group("Модуль 3", m3 => m3
            .Item<Module3SignalsPage>("Сигналы"))
        .Group("Модуль 4", m4 => m4
            .Item<Module4SignalsPage>("Сигналы"))
        .Item<ConnectionSettingsPage>("Сеть")
        .Build();
    }

    public IReadOnlyList<NavigationItem> Navigation { get; }

    public ObservableCollection<LogEntry> Logs => DataGridTarget.LogEntries;

    public ObservableCollection<object> Alarms { get; } = new();

    public ICommand CloseButtonCommand { get; } = new RelayCommand(_ =>
    {
        var result = UserMessageBox.Show(
            "Вы уверены, что хотите выйти из программы?",
            "Подтверждение выхода",
            MBType.Info,
            MBButtons.Okcancel);
        if (result == MBResult.Ok)
            System.Windows.Application.Current.Shutdown();
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
