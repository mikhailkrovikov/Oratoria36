namespace Oratoria.Application.Connection;

public sealed class ConnectionFile
{
    public ConnectionEndpoint Module1 { get; set; } = new();
    public ConnectionEndpoint Module2 { get; set; } = new();
    public ConnectionEndpoint Module3 { get; set; } = new();
    public ConnectionEndpoint Module4 { get; set; } = new();
    public ConnectionEndpoint Transport { get; set; } = new();
}
