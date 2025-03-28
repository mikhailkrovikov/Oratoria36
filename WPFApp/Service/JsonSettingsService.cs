using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Oratoria36.Service
{
    public class JsonSettingsService
    {
        public string SettingsFolder { get; }

        public JsonSettingsService()
        {
            SettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
            }
        }

        public async Task SaveSettingsAsync<T>(T settings, string fileName)
        {
            try
            {
                string filePath = Path.Combine(SettingsFolder, fileName);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(settings, options);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении настроек: {ex.Message}");
                throw;
            }
        }

        public async Task<T> LoadSettingsAsync<T>(string fileName) where T : new()
        {
            try
            {
                string filePath = Path.Combine(SettingsFolder, fileName);
                if (!File.Exists(filePath))
                {
                    return new T();
                }

                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<T>(json) ?? new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке настроек: {ex.Message}");
                return new T();
            }
        }
    }
}
