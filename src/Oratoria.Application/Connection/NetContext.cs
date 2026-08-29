using Oratoria.Domain.Connection;
using Microsoft.Extensions.Logging;

namespace Oratoria.Application.Connection;

public sealed class NetContext
{
    public ModbusTCPConfig Module1 { get; }
    public ModbusTCPConfig Module2 { get; }
    public ModbusTCPConfig Module3 { get; }
    public ModbusTCPConfig Module4 { get; }
    public ModbusTCPConfig TransportModule { get; }

    public NetContext(ILoggerFactory loggers)
    {
        var logger = loggers.CreateLogger("Подключение");
        Module1 = Config("192.168.0.102", 502, logger);
        Module2 = Config("192.168.0.103", 502, logger);
        Module3 = Config("192.168.0.104", 502, logger);
        Module4 = Config("192.168.0.105", 502, logger);
        TransportModule = Config("192.168.0.106", 502, logger);
    }

    private static ModbusTCPConfig Config(string ip, int port, ILogger logger)
    {
        var config = new ModbusTCPConfig(new object(), logger);
        config.IP = ip;
        config.Port = port;
        return config;
    }
}