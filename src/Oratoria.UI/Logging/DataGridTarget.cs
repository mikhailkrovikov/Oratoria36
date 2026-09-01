using System.Collections.ObjectModel;
using System.Windows;
using NLog;
using NLog.Targets;

namespace Oratoria.UI.Logging;

[Target("DataGrid")]
public sealed class DataGridTarget : TargetWithLayout
{
    private const int Limit = 3000;

    public static ObservableCollection<LogEntry> LogEntries { get; } = new();

    protected override void Write(LogEventInfo logEvent)
    {
        var entry = new LogEntry(logEvent);
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher is null)
            return;

        dispatcher.BeginInvoke(() =>
        {
            LogEntries.Add(entry);
            while (LogEntries.Count > Limit)
                LogEntries.RemoveAt(0);
        });
    }
}
