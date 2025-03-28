﻿using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NLog;

namespace Oratoria36.Models
{
    public class ModuleManager
    {
        private static readonly Logger _logger = LogManager.GetLogger("ModuleManager");
        private static ModuleManager _instance;
        private readonly string _settingsPath;
        public static ModuleManager Instance => _instance ??= new ModuleManager();
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

        private ModuleManager()
        {
            Module1.ModuleId = 1;
            Module2.ModuleId = 2;
            Module3.ModuleId = 3;
            Module4.ModuleId = 4;
            TransportModule.ModuleId = 5;

            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", "ConnectionSettings.json");
            _logger.Info($"Путь к файлу настроек: {_settingsPath}");

            LoadConnectionSettings();

            InitializeModules();

            Module1.PropertyChanged += ModulePropertyChanged;
            Module2.PropertyChanged += ModulePropertyChanged;
            Module3.PropertyChanged += ModulePropertyChanged;
            Module4.PropertyChanged += ModulePropertyChanged;
            TransportModule.PropertyChanged += ModulePropertyChanged;

            _logger.Info("ModuleManager инициализирован");
        }

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
                    _logger.Info($"Создана директория для настроек: {directory}");
                }
                if (File.Exists(_settingsPath))
                {
                    string json = File.ReadAllText(_settingsPath);
                    _logger.Info($"Загружен файл настроек: {json}");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };

                    var settings = JsonSerializer.Deserialize<ConnectionSettings>(json, options);

                    if (settings != null)
                    {
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
                        _logger.Warn("Файл настроек существует, но десериализация вернула null");
                    }
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
            _logger.Info($"Инициализация Module1: IP={Module1IP}, Port={Module1Port}");
            Module1.IP = Module1IP;
            Module1.Port = Module1Port;

            _logger.Info($"Инициализация Module2: IP={Module2IP}, Port={Module2Port}");
            Module2.IP = Module2IP;
            Module2.Port = Module2Port;

            _logger.Info($"Инициализация Module3: IP={Module3IP}, Port={Module3Port}");
            Module3.IP = Module3IP;
            Module3.Port = Module3Port;

            _logger.Info($"Инициализация Module4: IP={Module4IP}, Port={Module4Port}");
            Module4.IP = Module4IP;
            Module4.Port = Module4Port;

            _logger.Info($"Инициализация TransportModule: IP={TransportModuleIP}, Port={TransportModulePort}");
            TransportModule.IP = TransportModuleIP;
            TransportModule.Port = TransportModulePort;

            _logger.Info("Модули инициализированы");
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

                _logger.Info($"Настройки соединения сохранены: {json}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при сохранении настроек соединения");
            }
        }

        public async Task ConnectAllAsync()
        {
            try
            {
                var tasks = new List<Task>();

                if (!string.IsNullOrEmpty(Module1.IP))
                    tasks.Add(Module1.InitializeModbusAsync(Module1.IP));

                if (!string.IsNullOrEmpty(Module2.IP))
                    tasks.Add(Module2.InitializeModbusAsync(Module2.IP));

                if (!string.IsNullOrEmpty(Module3.IP))
                    tasks.Add(Module3.InitializeModbusAsync(Module3.IP));

                if (!string.IsNullOrEmpty(Module4.IP))
                    tasks.Add(Module4.InitializeModbusAsync(Module4.IP));

                if (!string.IsNullOrEmpty(TransportModule.IP))
                    tasks.Add(TransportModule.InitializeModbusAsync(TransportModule.IP));

                if (tasks.Count > 0)
                    await Task.WhenAll(tasks);

                ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);

                _logger.Info("Попытка подключения ко всем модулям завершена");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при подключении к модулям");
            }
        }

        public void DisconnectAll()
        {
            try
            {
                Module1.CloseConnection();
                Module2.CloseConnection();
                Module3.CloseConnection();
                Module4.CloseConnection();
                TransportModule.CloseConnection();

                ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);

                _logger.Info("Все модули отключены");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при отключении модулей");
            }
        }
    }

    public class ConnectionSettings
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
}
