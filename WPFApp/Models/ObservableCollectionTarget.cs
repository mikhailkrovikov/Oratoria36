using System.Collections.ObjectModel;
using NLog;
using NLog.Targets;

[Target("ObservableCollection")]
public sealed class ObservableCollectionTarget : TargetWithLayout
{
    public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();

    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = this.Layout.Render(logEvent);
        Logs.Add(logMessage);
    }
}