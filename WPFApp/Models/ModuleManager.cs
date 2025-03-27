using System.Threading.Tasks;

namespace Oratoria36.Models
{
    public class ModuleManager
    {
        public ModuleConfig Module1 { get; private set; } = new ModuleConfig();
        public ModuleConfig Module2 { get; private set; } = new ModuleConfig();
        public ModuleConfig Module3 { get; private set; } = new ModuleConfig();
        public ModuleConfig Module4 { get; private set; } = new ModuleConfig();
        public ModuleConfig TransportModule { get; private set; } = new ModuleConfig();

        public async Task ConnectAllAsync()
        {
            await Task.WhenAll(
                Module1.InitializeModbusAsync(Module1.IP),
                Module2.InitializeModbusAsync(Module2.IP),
                Module3.InitializeModbusAsync(Module3.IP),
                Module4.InitializeModbusAsync(Module4.IP),
                TransportModule.InitializeModbusAsync(TransportModule.IP));
        }

        public void DisconnectAll()
        {
            Module1.CloseConnection();
            Module2.CloseConnection();
            Module3.CloseConnection();
            Module4.CloseConnection();
            TransportModule.CloseConnection();
        }
    }
}