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

        public JsonSettingsService()
        {
            SettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");

            if (!Directory.Exists(SettingsFolder))
            {
                _logger.Info($"Создание папки настроек: {SettingsFolder}");
                Directory.CreateDirectory(SettingsFolder);
            }
        }

        public async Task SaveSettingsAsync<T>(T settings, string fileName)
        {
            try
            {
                string filePath = Path.Combine(SettingsFolder, fileName);
                _logger.Info($"Сохранение настроек в файл: {filePath}");

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(settings, options);
                await File.WriteAllTextAsync(filePath, jsonString);

                _logger.Info($"Файл настроек {fileName} успешно записан");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при сохранении настроек в файл {fileName}");
                throw;
            }
        }

        public async Task<T> LoadSettingsAsync<T>(string fileName, T defaultSettings = default)
        {
            string filePath = Path.Combine(SettingsFolder, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    _logger.Info($"Загрузка настроек из файла: {filePath}");
                    string jsonString = await File.ReadAllTextAsync(filePath);

                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        var result = JsonSerializer.Deserialize<T>(jsonString);
                        _logger.Info($"Настройки из файла {fileName} успешно загружены");
                        return result;
                    }
                }

                _logger.Warn($"Файл настроек {fileName} не найден или пуст. Используются настройки по умолчанию.");
                return defaultSettings;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при загрузке настроек из файла {fileName}");
                return defaultSettings;
            }
        }
    }
}
