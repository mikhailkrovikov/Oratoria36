using System.Threading.Tasks;
using Oratoria36.Service;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;

namespace Oratoria36.Models
{
    public class ModuleManager
    {
        private readonly JsonSettingsService _settingsService;
        private const string CONNECTION_SETTINGS_FILE = "ConnectionSettings.json";

        // Свойства для сериализации/десериализации JSON
        public string Module1IP { get; set; } = "192.168.0.102";
        public int Module1Port { get; set; } = 502;
        public string Module2IP { get; set; }
        public int Module2Port { get; set; } = 502;
        public string Module3IP { get; set; }
        public int Module3Port { get; set; } = 502;
        public string Module4IP { get; set; }
        public int Module4Port { get; set; } = 502;
        public string TransportModuleIP { get; set; }
        public int TransportModulePort { get; set; } = 502;

        // Игнорируем эти свойства при сериализации в JSON
        [JsonIgnore]
        public ModuleConfig Module1 { get; private set; } = new ModuleConfig();
        [JsonIgnore]
        public ModuleConfig Module2 { get; private set; } = new ModuleConfig();
        [JsonIgnore]
        public ModuleConfig Module3 { get; private set; } = new ModuleConfig();
        [JsonIgnore]
        public ModuleConfig Module4 { get; private set; } = new ModuleConfig();
        [JsonIgnore]
        public ModuleConfig TransportModule { get; private set; } = new ModuleConfig();

        public ModuleManager()
        {
            _settingsService = new JsonSettingsService();
            LoadConnectionSettings();
            InitializeModules();
        }

        private void LoadConnectionSettings()
        {
            // Вместо десериализации в объект ModuleManager, используем анонимный тип
            // или словарь для временного хранения данных
            try
            {
                string filePath = Path.Combine(_settingsService.SettingsFolder, CONNECTION_SETTINGS_FILE);
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var tempSettings = JsonSerializer.Deserialize<ModuleManagerSettings>(json);

                    if (tempSettings != null)
                    {
                        // Копируем значения из временного объекта
                        Module1IP = tempSettings.Module1IP ?? Module1IP;
                        Module1Port = tempSettings.Module1Port;
                        Module2IP = tempSettings.Module2IP;
                        Module2Port = tempSettings.Module2Port;
                        Module3IP = tempSettings.Module3IP;
                        Module3Port = tempSettings.Module3Port;
                        Module4IP = tempSettings.Module4IP;
                        Module4Port = tempSettings.Module4Port;
                        TransportModuleIP = tempSettings.TransportModuleIP;
                        TransportModulePort = tempSettings.TransportModulePort;
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка при загрузке настроек: {ex.Message}");
            }
        }

        // Вспомогательный класс только для десериализации
        private class ModuleManagerSettings
        {
            public string Module1IP { get; set; }
            public int Module1Port { get; set; }
            public string Module2IP { get; set; }
            public int Module2Port { get; set; }
            public string Module3IP { get; set; }
            public int Module3Port { get; set; }
            public string Module4IP { get; set; }
            public int Module4Port { get; set; }
            public string TransportModuleIP { get; set; }
            public int TransportModulePort { get; set; }
        }

        private void InitializeModules()
        {
            Module1.IP = Module1IP;
            Module1.Port = Module1Port;

            Module2.IP = Module2IP;
            Module2.Port = Module2Port;

            Module3.IP = Module3IP;
            Module3.Port = Module3Port;

            Module4.IP = Module4IP;
            Module4.Port = Module4Port;

            TransportModule.IP = TransportModuleIP;
            TransportModule.Port = TransportModulePort;
        }

        public async Task SaveConnectionSettingsAsync()
        {
            // Обновляем свойства для сохранения из объектов ModuleConfig
            Module1IP = Module1.IP;
            Module1Port = Module1.Port;
            Module2IP = Module2.IP;
            Module2Port = Module2.Port;
            Module3IP = Module3.IP;
            Module3Port = Module3.Port;
            Module4IP = Module4.IP;
            Module4Port = Module4.Port;
            TransportModuleIP = TransportModule.IP;
            TransportModulePort = TransportModule.Port;

            await _settingsService.SaveSettingsAsync(this, CONNECTION_SETTINGS_FILE);
        }

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