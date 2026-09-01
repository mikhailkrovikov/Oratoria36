using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Oratoria.Application.Connection;
using Oratoria.Domain.Connection;
using Oratoria.UI.Helpers;

namespace Oratoria.UI.ViewModels;

public sealed class ConnectionSettingsVM : INotifyPropertyChanged
{
    private bool _busy1;
    private bool _busy2;
    private bool _busy3;
    private bool _busy4;
    private bool _busyTransport;

    public ConnectionSettingsVM(NetContext net)
    {
        Module1 = net.Module1;
        Module2 = net.Module2;
        Module3 = net.Module3;
        Module4 = net.Module4;
        Transport = net.TransportModule;
        CurrentHostIP = ResolveHostIp();

        foreach (var config in new[] { Module1, Module2, Module3, Module4, Transport })
            config.PropertyChanged += (_, _) => UpdateStatus();

        ConnectCommandModule1 = ConnectCmd(() => _busy1, v => _busy1 = v, Module1);
        DisconnectCommandModule1 = DisconnectCmd(() => _busy1, v => _busy1 = v, Module1);
        ApplySettingsCommandModule1 = ApplyCmd(() => NewIPModule1, v => NewIPModule1 = v, () => NewPortModule1, v => NewPortModule1 = v, Module1);

        ConnectCommandModule2 = ConnectCmd(() => _busy2, v => _busy2 = v, Module2);
        DisconnectCommandModule2 = DisconnectCmd(() => _busy2, v => _busy2 = v, Module2);
        ApplySettingsCommandModule2 = ApplyCmd(() => NewIPModule2, v => NewIPModule2 = v, () => NewPortModule2, v => NewPortModule2 = v, Module2);

        ConnectCommandModule3 = ConnectCmd(() => _busy3, v => _busy3 = v, Module3);
        DisconnectCommandModule3 = DisconnectCmd(() => _busy3, v => _busy3 = v, Module3);
        ApplySettingsCommandModule3 = ApplyCmd(() => NewIPModule3, v => NewIPModule3 = v, () => NewPortModule3, v => NewPortModule3 = v, Module3);

        ConnectCommandModule4 = ConnectCmd(() => _busy4, v => _busy4 = v, Module4);
        DisconnectCommandModule4 = DisconnectCmd(() => _busy4, v => _busy4 = v, Module4);
        ApplySettingsCommandModule4 = ApplyCmd(() => NewIPModule4, v => NewIPModule4 = v, () => NewPortModule4, v => NewPortModule4 = v, Module4);

        ConnectCommandTransport = ConnectCmd(() => _busyTransport, v => _busyTransport = v, Transport);
        DisconnectCommandTransport = DisconnectCmd(() => _busyTransport, v => _busyTransport = v, Transport);
        ApplySettingsCommandTransport = ApplyCmd(() => NewIPTransport, v => NewIPTransport = v, () => NewPortTransport, v => NewPortTransport = v, Transport);

        UpdateStatus();
    }

    public string CurrentHostIP { get; }

    public ModbusTCPConfig Module1 { get; }
    public ModbusTCPConfig Module2 { get; }
    public ModbusTCPConfig Module3 { get; }
    public ModbusTCPConfig Module4 { get; }
    public ModbusTCPConfig Transport { get; }

    public string Module1Status { get; private set; } = "Отключено";
    public string Module2Status { get; private set; } = "Отключено";
    public string Module3Status { get; private set; } = "Отключено";
    public string Module4Status { get; private set; } = "Отключено";
    public string TransportStatus { get; private set; } = "Отключено";

    public string Module1CurrentIP { get; private set; } = "—";
    public string Module2CurrentIP { get; private set; } = "—";
    public string Module3CurrentIP { get; private set; } = "—";
    public string Module4CurrentIP { get; private set; } = "—";
    public string TransportCurrentIP { get; private set; } = "—";

    public int Module1CurrentPort { get; private set; }
    public int Module2CurrentPort { get; private set; }
    public int Module3CurrentPort { get; private set; }
    public int Module4CurrentPort { get; private set; }
    public int TransportCurrentPort { get; private set; }

    private string _newIPModule1 = string.Empty;
    private string _newPortModule1 = string.Empty;
    private string _newIPModule2 = string.Empty;
    private string _newPortModule2 = string.Empty;
    private string _newIPModule3 = string.Empty;
    private string _newPortModule3 = string.Empty;
    private string _newIPModule4 = string.Empty;
    private string _newPortModule4 = string.Empty;
    private string _newIPTransport = string.Empty;
    private string _newPortTransport = string.Empty;

    public string NewIPModule1 { get => _newIPModule1; set => Set(ref _newIPModule1, value); }
    public string NewPortModule1 { get => _newPortModule1; set => Set(ref _newPortModule1, value); }
    public string NewIPModule2 { get => _newIPModule2; set => Set(ref _newIPModule2, value); }
    public string NewPortModule2 { get => _newPortModule2; set => Set(ref _newPortModule2, value); }
    public string NewIPModule3 { get => _newIPModule3; set => Set(ref _newIPModule3, value); }
    public string NewPortModule3 { get => _newPortModule3; set => Set(ref _newPortModule3, value); }
    public string NewIPModule4 { get => _newIPModule4; set => Set(ref _newIPModule4, value); }
    public string NewPortModule4 { get => _newPortModule4; set => Set(ref _newPortModule4, value); }
    public string NewIPTransport { get => _newIPTransport; set => Set(ref _newIPTransport, value); }
    public string NewPortTransport { get => _newPortTransport; set => Set(ref _newPortTransport, value); }

    public ICommand ConnectCommandModule1 { get; }
    public ICommand DisconnectCommandModule1 { get; }
    public ICommand ApplySettingsCommandModule1 { get; }
    public ICommand ConnectCommandModule2 { get; }
    public ICommand DisconnectCommandModule2 { get; }
    public ICommand ApplySettingsCommandModule2 { get; }
    public ICommand ConnectCommandModule3 { get; }
    public ICommand DisconnectCommandModule3 { get; }
    public ICommand ApplySettingsCommandModule3 { get; }
    public ICommand ConnectCommandModule4 { get; }
    public ICommand DisconnectCommandModule4 { get; }
    public ICommand ApplySettingsCommandModule4 { get; }
    public ICommand ConnectCommandTransport { get; }
    public ICommand DisconnectCommandTransport { get; }
    public ICommand ApplySettingsCommandTransport { get; }

    private ICommand ConnectCmd(Func<bool> busy, Action<bool> setBusy, ModbusTCPConfig config)
        => new RelayCommand(async _ => await ConnectAsync(busy, setBusy, config), _ => !busy() && !config.IsConnected);

    private ICommand DisconnectCmd(Func<bool> busy, Action<bool> setBusy, ModbusTCPConfig config)
        => new RelayCommand(_ =>
        {
            setBusy(true);
            CommandManager.InvalidateRequerySuggested();
            try { config.CloseConnection(); }
            finally { setBusy(false); UpdateStatus(); }
        }, _ => !busy() && config.IsConnected);

    private ICommand ApplyCmd(
        Func<string> getIp, Action<string> setIp,
        Func<string> getPort, Action<string> setPort,
        ModbusTCPConfig config)
        => new RelayCommand(_ =>
        {
            var ip = string.IsNullOrWhiteSpace(getIp()) ? config.IP : getIp().Trim();
            var port = config.Port;
            if (!string.IsNullOrWhiteSpace(getPort()) && !int.TryParse(getPort(), out port))
                return;
            if (ip == config.IP && port == config.Port)
                return;
            config.IP = ip;
            config.Port = port;
            setIp(string.Empty);
            setPort(string.Empty);
            UpdateStatus();
        });

    private async Task ConnectAsync(Func<bool> busy, Action<bool> setBusy, ModbusTCPConfig config)
    {
        setBusy(true);
        CommandManager.InvalidateRequerySuggested();
        try { await config.Connect(); }
        finally
        {
            setBusy(false);
            UpdateStatus();
        }
    }

    private void UpdateStatus()
    {
        Module1Status = Status(Module1);
        Module2Status = Status(Module2);
        Module3Status = Status(Module3);
        Module4Status = Status(Module4);
        TransportStatus = Status(Transport);
        Module1CurrentIP = Module1.IP ?? "—";
        Module2CurrentIP = Module2.IP ?? "—";
        Module3CurrentIP = Module3.IP ?? "—";
        Module4CurrentIP = Module4.IP ?? "—";
        TransportCurrentIP = Transport.IP ?? "—";
        Module1CurrentPort = Module1.Port;
        Module2CurrentPort = Module2.Port;
        Module3CurrentPort = Module3.Port;
        Module4CurrentPort = Module4.Port;
        TransportCurrentPort = Transport.Port;
        OnPropertyChanged(nameof(Module1Status));
        OnPropertyChanged(nameof(Module2Status));
        OnPropertyChanged(nameof(Module3Status));
        OnPropertyChanged(nameof(Module4Status));
        OnPropertyChanged(nameof(TransportStatus));
        OnPropertyChanged(nameof(Module1CurrentIP));
        OnPropertyChanged(nameof(Module2CurrentIP));
        OnPropertyChanged(nameof(Module3CurrentIP));
        OnPropertyChanged(nameof(Module4CurrentIP));
        OnPropertyChanged(nameof(TransportCurrentIP));
        OnPropertyChanged(nameof(Module1CurrentPort));
        OnPropertyChanged(nameof(Module2CurrentPort));
        OnPropertyChanged(nameof(Module3CurrentPort));
        OnPropertyChanged(nameof(Module4CurrentPort));
        OnPropertyChanged(nameof(TransportCurrentPort));
        CommandManager.InvalidateRequerySuggested();
    }

    private static string Status(ModbusTCPConfig config) => config.IsConnected ? "Подключено" : "Отключено";

    private static string ResolveHostIp()
    {
        try
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName());
            var ipv4 = addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            return (ipv4 ?? addresses.LastOrDefault())?.ToString() ?? "—";
        }
        catch
        {
            return "—";
        }
    }

    private void Set(ref string field, string value, [CallerMemberName] string? name = null)
    {
        if (field == value)
            return;
        field = value;
        OnPropertyChanged(name);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
