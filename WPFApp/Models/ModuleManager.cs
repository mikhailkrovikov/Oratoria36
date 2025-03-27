using System;
using System.Threading.Tasks;
using Oratoria36.Service;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;
using NLog;

namespace Oratoria36.Models
{
    public class ModuleManager
    {
        private static readonly Logger _logger = LogManager.GetLogger("Соединение");
        private static readonly Lazy<ModuleManager> _instance =
            new Lazy<ModuleManager>(() => new ModuleManager());
        public static ModuleManager Instance => _instance.Value;
        public event Action<string> ConnectionStatusChanged;
        private readonly JsonSettingsService _settingsService;
        private const string CONNECTION_SETTINGS_FILE = "ConnectionSettings.json";

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

        private ModuleManager()
        {
            _settingsService = new JsonSettingsService();
            LoadConnectionSettings();
            InitializeModules();
        }

        private void LoadConnectionSettings()
        {
            try
            {
                string filePath = Path.Combine(_settingsService.SettingsFolder, CONNECTION_SETTINGS_FILE);
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var tempSettings = JsonSerializer.Deserialize<ModuleManagerSettings>(json);

                    if (tempSettings != null)
                    {
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
                _logger.Error(ex, "Ошибка при загрузке настроек");
            }
        }
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
            try
            {
                _logger.Info("Начало автоматического подключения к модулям...");

                await Task.WhenAll(
                    Module1.InitializeModbusAsync(Module1.IP),
                    Module2.InitializeModbusAsync(Module2.IP),
                    Module3.InitializeModbusAsync(Module3.IP),
                    Module4.InitializeModbusAsync(Module4.IP),
                    TransportModule.InitializeModbusAsync(TransportModule.IP));

                var status = Module1.IsConnected ? "Подключено" : "Отключено";
                ConnectionStatusChanged?.Invoke(status);
                if (Module1.IsConnected || Module2.IsConnected || Module3.IsConnected || Module4.IsConnected || TransportModule.IsConnected)
                {
                    _logger.Info("Автоматическое подключение завершено");
                }
                else
                {
                    _logger.Error("Автоматическое подключение завершено с ошибкой");
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при автоматическом подключении к модулям");
                ConnectionStatusChanged?.Invoke("Ошибка");
            }
        }

        public void DisconnectAll()
        {
            try
            {
                _logger.Info("Отключение всех модулей");

                Module1.CloseConnection();
                Module2.CloseConnection();
                Module3.CloseConnection();
                Module4.CloseConnection();
                TransportModule.CloseConnection();

                ConnectionStatusChanged?.Invoke("Отключено");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при отключении модулей");
                ConnectionStatusChanged?.Invoke("Ошибка отключения");
            }
        }
    }
}
