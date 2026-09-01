using NLog;

namespace Oratoria.UI.Logging;

public sealed class LogEntry
{
    public DateTime Timestamp { get; }
    public string Level { get; }
    public string Module { get; }
    public string Message { get; }
    public string Logger { get; }
    public string? Exception { get; }

    public LogEntry(LogEventInfo logEvent)
    {
        Timestamp = logEvent.TimeStamp;
        Level = NormalizeLevel(logEvent.Level?.Name);
        if (logEvent.Properties.TryGetValue("module", out var module))        
            Module = module?.ToString() ?? "-";
        
        else    
            Module = "-";
        
        Message = logEvent.FormattedMessage;
        Logger = logEvent.LoggerName;
        Exception = logEvent.Exception?.ToString();
    }

    public static string NormalizeLevel(string? levelName) => levelName switch
    {
        "Trace" or "Трассировка" => "Трассировка",
        "Debug" or "Отладка" => "Отладка",
        "Info" or "Инфо" => "Инфо",
        "Warn" or "Предупреждение" => "Предупреждение",
        "Error" or "Ошибка" => "Ошибка",
        "Fatal" or "Критическая" => "Критическая",
        _ => string.IsNullOrWhiteSpace(levelName) ? "Инфо" : levelName
    };
}
