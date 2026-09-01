using System.Text.Json;

namespace Oratoria.Infrastructure;

public sealed class JsonFileStore<T> where T : class
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
    };

    private readonly string _path;

    public JsonFileStore(string path)
    {
        _path = path;
    }

    public T? Load()
    {
        try
        {
            if (!File.Exists(_path))
                return null;

            return JsonSerializer.Deserialize<T>(File.ReadAllText(_path), Options);
        }
        catch
        {
            return null;
        }
    }

    public void Save(T value)
    {
        var directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        var tmp = _path + ".tmp";
        File.WriteAllText(tmp, JsonSerializer.Serialize(value, Options));
        File.Move(tmp, _path, overwrite: true);
    }
}
