using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;

namespace Oratoria36.Service
{
    public class JsonSettingsService
    {
        private static readonly Logger _logger = LogManager.GetLogger("Settings");

        public string SettingsFolder { get; }

        public JsonSettingsService(string settingsFolder = "Settings")
        {
            SettingsFolder = settingsFolder;
            EnsureSettingsFolderExists();
        }

        private void EnsureSettingsFolderExists()
        {
            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
                _logger.Info($"Создана папка для настроек: {SettingsFolder}");
            }
        }

        public T LoadSettings<T>(string fileName) where T : new()
        {
            string filePath = Path.Combine(SettingsFolder, fileName);

            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.Info($"Файл настроек {fileName} не найден. Будут использованы настройки по умолчанию.");
                    return new T();
                }

                string json = File.ReadAllText(filePath);
                var settings = JsonSerializer.Deserialize<T>(json);
                _logger.Info($"Настройки успешно загружены из {fileName}");
                return settings ?? new T();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при загрузке настроек из {fileName}");
                return new T();
            }
        }

        public async Task SaveSettingsAsync<T>(T settings, string fileName)
        {
            string filePath = Path.Combine(SettingsFolder, fileName);

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                await File.WriteAllTextAsync(filePath, json);
                _logger.Info($"Настройки успешно сохранены в {fileName}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при сохранении настроек в {fileName}");
            }
        }
    }
}