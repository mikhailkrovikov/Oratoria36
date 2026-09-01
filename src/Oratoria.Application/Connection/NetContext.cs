using Microsoft.Extensions.Logging;
using Oratoria.Domain.Connection;
using Oratoria.Infrastructure;

namespace Oratoria.Application.Connection;

public class NetContext
{
    private readonly JsonFileStore<ConnectionFile> _store;

    public ModbusTCPConfig Module1 { get; }
    public ModbusTCPConfig Module2 { get; }
    public ModbusTCPConfig Module3 { get; }
    public ModbusTCPConfig Module4 { get; }
    public ModbusTCPConfig TransportModule { get; }

    public NetContext(ILoggerFactory loggers, JsonFileStore<ConnectionFile> store)
    {
        _store = store;
        var logger = loggers.CreateLogger("Подключение");
        var file = store.Load() ?? new ConnectionFile
        {
            Module1 = new() { Ip = "192.168.0.102", Port = 502 },
            Module2 = new() { Ip = "192.168.0.103", Port = 502 },
            Module3 = new() { Ip = "192.168.0.104", Port = 502 },
            Module4 = new() { Ip = "192.168.0.105", Port = 502 },
            Transport = new() { Ip = "192.168.0.106", Port = 502 },
        };
        Module1 = new ModbusTCPConfig(new object(), logger) { IP = file.Module1.Ip, Port = file.Module1.Port };
        Module2 = new ModbusTCPConfig(new object(), logger) { IP = file.Module2.Ip, Port = file.Module2.Port };
        Module3 = new ModbusTCPConfig(new object(), logger) { IP = file.Module3.Ip, Port = file.Module3.Port };
        Module4 = new ModbusTCPConfig(new object(), logger) { IP = file.Module4.Ip, Port = file.Module4.Port };
        TransportModule = new ModbusTCPConfig(new object(), logger) { IP = file.Transport.Ip, Port = file.Transport.Port };
    }

    public void Save()
    {
        _store.Save(new ConnectionFile
        {
            Module1 = new() { Ip = Module1.IP ?? "", Port = Module1.Port },
            Module2 = new() { Ip = Module2.IP ?? "", Port = Module2.Port },
            Module3 = new() { Ip = Module3.IP ?? "", Port = Module3.Port },
            Module4 = new() { Ip = Module4.IP ?? "", Port = Module4.Port },
            Transport = new() { Ip = TransportModule.IP ?? "", Port = TransportModule.Port },
        });
    }
}
