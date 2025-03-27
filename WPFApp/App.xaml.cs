using System;
using System.Windows;
using NLog;
using NLog.Config;
using NLog.Targets;
using Oratoria36.Service;

namespace Oratoria36
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureNLog();
        }

        private void ConfigureNLog()
        {
            var config = new LoggingConfiguration();
            Target.Register<DataGridTarget>("DataGrid");
            var dataGridTarget = new DataGridTarget
            {
                Name = "dataGrid",
                Layout = "${message}"
            };
            var fileTarget = new FileTarget
            {
                Name = "file",
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=ToString}"
            };
            config.AddTarget(dataGridTarget);
            config.AddTarget(fileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, dataGridTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }
    }
}