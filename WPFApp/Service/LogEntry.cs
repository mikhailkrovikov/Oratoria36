using NLog.Layouts;
using NLog.Targets;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Oratoria36.Service
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Exception { get; set; }

        public LogEntry(DateTime timestamp, string level, string message, string logger, string exception = null)
        {
            Timestamp = timestamp;
            Level = level;
            Message = message;
            Logger = logger;
            Exception = exception;
        }
    }
    [Target("DataGrid")]
    public class DataGridTarget : TargetWithLayout
    {
        private static readonly ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();
        private static readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        public static ObservableCollection<LogEntry> LogEntries => _logEntries;

        protected override void Write(LogEventInfo logEvent)
        {
            string message = Layout.Render(logEvent);
            string exception = logEvent.Exception?.ToString();

            var logEntry = new LogEntry(
                logEvent.TimeStamp,
                logEvent.Level.Name,
                message,
                logEvent.LoggerName,
                exception
            );

            // Добавляем запись в коллекцию через диспетчер UI-потока
            _dispatcher.BeginInvoke(new Action(() =>
            {
                _logEntries.Add(logEntry);
            }));
        }
    }
}
