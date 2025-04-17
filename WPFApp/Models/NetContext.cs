using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NLog;

namespace Oratoria36.Models
{
    public class NetContext
    {
        private static NetContext _instance;
        public static NetContext Instance => GetInstance();

        private static NetContext GetInstance()
        {
            if (_instance == null)
                _instance = new NetContext();
            return _instance;
        }

        private NetContext()
        {

            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", "ConnectionSettings.json");
            _logger.Info($"Путь к файлу настроек: {_settingsPath}");

            LoadConnectionSettings();

            InitializeModules();

            Module1.PropertyChanged += ModulePropertyChanged;
            Module2.PropertyChanged += ModulePropertyChanged;
            Module3.PropertyChanged += ModulePropertyChanged;
            Module4.PropertyChanged += ModulePropertyChanged;
            TransportModule.PropertyChanged += ModulePropertyChanged;

        }

        private static readonly Logger _logger = LogManager.GetLogger("NetContext");
        private readonly string _settingsPath;

        public string Module1IP { get; set; } = "192.168.0.102";
        public int Module1Port { get; set; } = 502;
        public string Module2IP { get; set; } = "192.168.0.103";
        public int Module2Port { get; set; } = 502;
        public string Module3IP { get; set; } = "192.168.0.104";
        public int Module3Port { get; set; } = 502;
        public string Module4IP { get; set; } = "192.168.0.105";
        public int Module4Port { get; set; } = 502;
        public string TransportModuleIP { get; set; } = "192.168.0.106";
        public int TransportModulePort { get; set; } = 502;

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

        public event EventHandler ConnectionStatusChanged;

        

        private void ModulePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ModuleConfig.IsConnected))
            {
                ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void LoadConnectionSettings()
        {
            try
            {
                string directory = Path.GetDirectoryName(_settingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (File.Exists(_settingsPath))
                {
                    string json = File.ReadAllText(_settingsPath);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };

                    var settings = JsonSerializer.Deserialize<ConnectionSettings>(json, options);


                    Module1IP = settings.Module1IP ?? Module1IP;
                    Module1Port = settings.Module1Port > 0 ? settings.Module1Port : Module1Port;
                    Module2IP = settings.Module2IP;
                    Module2Port = settings.Module2Port > 0 ? settings.Module2Port : Module2Port;
                    Module3IP = settings.Module3IP;
                    Module3Port = settings.Module3Port > 0 ? settings.Module3Port : Module3Port;
                    Module4IP = settings.Module4IP;
                    Module4Port = settings.Module4Port > 0 ? settings.Module4Port : Module4Port;
                    TransportModuleIP = settings.TransportModuleIP;
                    TransportModulePort = settings.TransportModulePort > 0 ? settings.TransportModulePort : TransportModulePort;
                    _logger.Info("Настройки соединения успешно загружены");
                }
                else
                {
                    _logger.Info("Файл настроек не найден, будут использованы значения по умолчанию");
                    SaveConnectionSettingsAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при загрузке настроек соединения");
            }
        }

        private void InitializeModules()
        {
            _logger.Info($"Инициализация Модуля 1: IP={Module1IP}, Port={Module1Port}");
            Module1.IP = Module1IP;
            Module1.Port = Module1Port;

            _logger.Info($"Инициализация Модуля 2: IP={Module2IP}, Port={Module2Port}");
            Module2.IP = Module2IP;
            Module2.Port = Module2Port;

            _logger.Info($"Инициализация Модуля 3: IP={Module3IP}, Port={Module3Port}");
            Module3.IP = Module3IP;
            Module3.Port = Module3Port;

            _logger.Info($"Инициализация Модуля 4: IP={Module4IP}, Port={Module4Port}");
            Module4.IP = Module4IP;
            Module4.Port = Module4Port;

            _logger.Info($"Инициализация Транспортного модуля: IP={TransportModuleIP}, Port={TransportModulePort}");
            TransportModule.IP = TransportModuleIP;
            TransportModule.Port = TransportModulePort;
        }

        public async Task SaveConnectionSettingsAsync()
        {
            try
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

                var settings = new ConnectionSettings
                {
                    Module1IP = Module1IP,
                    Module1Port = Module1Port,
                    Module2IP = Module2IP,
                    Module2Port = Module2Port,
                    Module3IP = Module3IP,
                    Module3Port = Module3Port,
                    Module4IP = Module4IP,
                    Module4Port = Module4Port,
                    TransportModuleIP = TransportModuleIP,
                    TransportModulePort = TransportModulePort
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);

                string directory = Path.GetDirectoryName(_settingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllTextAsync(_settingsPath, json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при сохранении настроек соединения");
            }
        }
    }
}
