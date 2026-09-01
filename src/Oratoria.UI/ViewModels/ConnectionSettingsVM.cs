using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using Oratoria.Application.Connection;
using Oratoria.Domain.Connection;
using Oratoria.UI.Helpers;

namespace Oratoria.UI.ViewModels;

public sealed class ConnectionSettingsVM
{
    private readonly NetContext _net;

    public ConnectionSettingsVM(NetContext net)
    {
        _net = net;
        Module1 = net.Module1;
        Module2 = net.Module2;
        Module3 = net.Module3;
        Module4 = net.Module4;
        Transport = net.TransportModule;
    }

    public string CurrentHostIP
    {
        get
        {
            string host = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(host);
            return addresses.Last().ToString();
        }
    }

    public ModbusTCPConfig Module1 { get; }
    public ModbusTCPConfig Module2 { get; }
    public ModbusTCPConfig Module3 { get; }
    public ModbusTCPConfig Module4 { get; }
    public ModbusTCPConfig Transport { get; }

    public string Module1Status => Module1.IsConnected ? "Подключено" : "Отключено";
    public string Module2Status => Module2.IsConnected ? "Подключено" : "Отключено";
    public string Module3Status => Module3.IsConnected ? "Подключено" : "Отключено";
    public string Module4Status => Module4.IsConnected ? "Подключено" : "Отключено";
    public string TransportStatus => Transport.IsConnected ? "Подключено" : "Отключено";

    public string? Module1CurrentIP => Module1.IP;
    public string? Module2CurrentIP => Module2.IP;
    public string? Module3CurrentIP => Module3.IP;
    public string? Module4CurrentIP => Module4.IP;
    public string? TransportCurrentIP => Transport.IP;

    public int Module1CurrentPort => Module1.Port;
    public int Module2CurrentPort => Module2.Port;
    public int Module3CurrentPort => Module3.Port;
    public int Module4CurrentPort => Module4.Port;
    public int TransportCurrentPort => Transport.Port;

    public string NewIPModule1 { get; set; } = string.Empty;
    public string NewPortModule1 { get; set; } = string.Empty;
    public string NewIPModule2 { get; set; } = string.Empty;
    public string NewPortModule2 { get; set; } = string.Empty;
    public string NewIPModule3 { get; set; } = string.Empty;
    public string NewPortModule3 { get; set; } = string.Empty;
    public string NewIPModule4 { get; set; } = string.Empty;
    public string NewPortModule4 { get; set; } = string.Empty;
    public string NewIPTransport { get; set; } = string.Empty;
    public string NewPortTransport { get; set; } = string.Empty;

    public ICommand ConnectCommandModule1
    {
        get => new RelayCommand(async (_) =>
        {
            await Module1.Connect();
        },
        (_) => !Module1.IsConnected);
    }

    public ICommand DisconnectCommandModule1
    {
        get => new RelayCommand((_) =>
        {
            Module1.CloseConnection();
        },
        (_) => Module1.IsConnected);
    }

    public ICommand ApplySettingsCommandModule1
    {
        get => new RelayCommand((_) =>
        {
            var ip = string.IsNullOrWhiteSpace(NewIPModule1) ? Module1.IP : NewIPModule1.Trim();
            var port = Module1.Port;
            if (!string.IsNullOrWhiteSpace(NewPortModule1) && !int.TryParse(NewPortModule1, out port))
                return;
            if (ip == Module1.IP && port == Module1.Port)
                return;
            Module1.IP = ip;
            Module1.Port = port;
            _net.Save();
            NewIPModule1 = string.Empty;
            NewPortModule1 = string.Empty;
        });
    }

    public ICommand ConnectCommandModule2
    {
        get => new RelayCommand(async (_) =>
        {
            await Module2.Connect();
        },
        (_) => !Module2.IsConnected);
    }

    public ICommand DisconnectCommandModule2
    {
        get => new RelayCommand((_) =>
        {
            Module2.CloseConnection();
        },
        (_) => Module2.IsConnected);
    }

    public ICommand ApplySettingsCommandModule2
    {
        get => new RelayCommand((_) =>
        {
            var ip = string.IsNullOrWhiteSpace(NewIPModule2) ? Module2.IP : NewIPModule2.Trim();
            var port = Module2.Port;
            if (!string.IsNullOrWhiteSpace(NewPortModule2) && !int.TryParse(NewPortModule2, out port))
                return;
            if (ip == Module2.IP && port == Module2.Port)
                return;
            Module2.IP = ip;
            Module2.Port = port;
            _net.Save();
            NewIPModule2 = string.Empty;
            NewPortModule2 = string.Empty;
        });
    }

    public ICommand ConnectCommandModule3
    {
        get => new RelayCommand(async (_) =>
        {
            await Module3.Connect();
        },
        (_) => !Module3.IsConnected);
    }

    public ICommand DisconnectCommandModule3
    {
        get => new RelayCommand((_) =>
        {
            Module3.CloseConnection();
        },
        (_) => Module3.IsConnected);
    }

    public ICommand ApplySettingsCommandModule3
    {
        get => new RelayCommand((_) =>
        {
            var ip = string.IsNullOrWhiteSpace(NewIPModule3) ? Module3.IP : NewIPModule3.Trim();
            var port = Module3.Port;
            if (!string.IsNullOrWhiteSpace(NewPortModule3) && !int.TryParse(NewPortModule3, out port))
                return;
            if (ip == Module3.IP && port == Module3.Port)
                return;
            Module3.IP = ip;
            Module3.Port = port;
            _net.Save();
            NewIPModule3 = string.Empty;
            NewPortModule3 = string.Empty;
        });
    }

    public ICommand ConnectCommandModule4
    {
        get => new RelayCommand(async (_) =>
        {
            await Module4.Connect();
        },
        (_) => !Module4.IsConnected);
    }

    public ICommand DisconnectCommandModule4
    {
        get => new RelayCommand((_) =>
        {
            Module4.CloseConnection();
        },
        (_) => Module4.IsConnected);
    }

    public ICommand ApplySettingsCommandModule4
    {
        get => new RelayCommand((_) =>
        {
            var ip = string.IsNullOrWhiteSpace(NewIPModule4) ? Module4.IP : NewIPModule4.Trim();
            var port = Module4.Port;
            if (!string.IsNullOrWhiteSpace(NewPortModule4) && !int.TryParse(NewPortModule4, out port))
                return;
            if (ip == Module4.IP && port == Module4.Port)
                return;
            Module4.IP = ip;
            Module4.Port = port;
            _net.Save();
            NewIPModule4 = string.Empty;
            NewPortModule4 = string.Empty;
        });
    }

    public ICommand ConnectCommandTransport
    {
        get => new RelayCommand(async (_) =>
        {
            await Transport.Connect();
        },
        (_) => !Transport.IsConnected);
    }

    public ICommand DisconnectCommandTransport
    {
        get => new RelayCommand((_) =>
        {
            Transport.CloseConnection();
        },
        (_) => Transport.IsConnected);
    }

    public ICommand ApplySettingsCommandTransport
    {
        get => new RelayCommand((_) =>
        {
            var ip = string.IsNullOrWhiteSpace(NewIPTransport) ? Transport.IP : NewIPTransport.Trim();
            var port = Transport.Port;
            if (!string.IsNullOrWhiteSpace(NewPortTransport) && !int.TryParse(NewPortTransport, out port))
                return;
            if (ip == Transport.IP && port == Transport.Port)
                return;
            Transport.IP = ip;
            Transport.Port = port;
            _net.Save();
            NewIPTransport = string.Empty;
            NewPortTransport = string.Empty;
        });
    }
}
