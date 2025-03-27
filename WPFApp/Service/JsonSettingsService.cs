using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;

namespace Oratoria36.Service
{
    public class JsonSettingsService
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetLogger("Settings");

        public string SettingsFolder { get; }

        public JsonSettingsService()
        {
            // Путь к папке Settings относительно исполняемого файла
            SettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");

            // Создаем папку, если она не существует
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

                _logger.Info("Файл настроек успешно записан");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при сохранении настроек");
                throw; // Перебрасываем исключение для обработки в вызывающем коде
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
                        _logger.Info("Настройки успешно загружены");
                        return result;
                    }
                }

                _logger.Warn($"Файл настроек не найден или пуст: {filePath}. Используются настройки по умолчанию.");
                return defaultSettings;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при загрузке настроек из файла: {filePath}");
                return defaultSettings;
            }
        }
    }
}