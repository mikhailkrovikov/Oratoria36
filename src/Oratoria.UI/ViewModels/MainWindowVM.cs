using Oratoria.Application.Gateway1;
using Oratoria.Application.Gateway2;
using Oratoria.Application.Module2;
using Oratoria.Application.TransportModule;
using Oratoria.Application.VacuumModule;
using Oratoria.UI.Controls.Controls.Navigation;
using Oratoria.UI.Controls.DialogWindows;
using Oratoria.UI.Logging;
using Oratoria.UI.Services;
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
    public MainWindowVM(
        AlarmService alarmService,
        Module2Context module2,
        VacuumContext vacuum,
        TransportContext transport,
        Gateway1Context gateway1,
        Gateway2Context gateway2)
    {
        alarmService.AddContext(module2);
        alarmService.AddContext(vacuum);
        alarmService.AddContext(transport);
        alarmService.AddContext(gateway1);
        alarmService.AddContext(gateway2);

        Alarms = alarmService.Items;

        Navigation = new NavigationBuilder()
        .AddSubButton<MainPage>("Главная")
        .AddMainButton("Транспорт", transport => transport
            .AddSubButton<TransportSignalsPage>("Сигналы"))
        .AddMainButton("Вакуумная система", vacuum => vacuum
            .AddSubButton<VacuumSignalsPage>("Сигналы"))
        .AddMainButton("Модуль 2", m2 => m2
            .AddSubButton<Module2MnemoPage>("Мнемосхема")
            .AddSubButton<Module2RecipePage>("Рецепт")
            .AddSubButton<Module2SignalsPage>("Сигналы")
            .AddSubButton<Module2SettingsPage>("Настройки")
            .AddSubButton<Module2LogsPage>("Журнал"))
        .AddMainButton("Модуль 3", m3 => m3
            .AddSubButton<Module3SignalsPage>("Сигналы"))
        .AddMainButton("Модуль 4", m4 => m4
            .AddSubButton<Module4SignalsPage>("Сигналы"))
        .AddSubButton<ConnectionSettingsPage>("Сеть")
        .Build();
    }

    public IReadOnlyList<NavigationItem> Navigation { get; }

    public ObservableCollection<LogEntry> Logs => DataGridTarget.LogEntries;

    public ObservableCollection<AlarmItem> Alarms { get; }

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
