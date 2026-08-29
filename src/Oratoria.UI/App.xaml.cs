using Oratoria.Application.Connection;
using Oratoria.Application.Module1.Signals;
using Oratoria.Application.Module2;
using Oratoria.Application.Module2.Signals;
using Oratoria.Application.Module3.Signals;
using Oratoria.Application.Module4.Signals;
using Oratoria.Application.Strategies;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.Application.VacuumModule.Signals;
using DigitalTwin;
using Oratoria.Domain.Signals.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;
using System.IO;
using System.Windows;
namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _services;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            EnsureLogDatabase();
            LogManager
                .Setup()
                .SetupExtensions(ext => ext.RegisterAssembly("NLog.Database"));
            LogManager
                .Setup()
                .LoadConfigurationFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config"));
            services.AddLogging(b => b.AddNLog());

            ConfigurateServices(services);

            _services = services.BuildServiceProvider();
            _services.GetRequiredService<MainWindow>().Show();
        }

        private static void ConfigurateServices(IServiceCollection services)
        {
            services.AddSingleton<NetContext>();
#if !RELEASE
            services.AddSingleton<TwinContext>();
            services.AddSingleton<IRegister>(sp => sp.GetRequiredService<TwinContext>().TModel);
            services.AddSingleton<DigitalTwinStrategy>();
#endif
            services.AddSingleton<Module1Signals>();
            services.AddSingleton<Module2Signals>();
            services.AddSingleton<Module3Signals>();
            services.AddSingleton<Module4Signals>();
            services.AddSingleton<TransportSignals>();
            services.AddSingleton<VacuumSignals>();

            services.AddSingleton<Module2Context>();

            services.AddSingleton<MainWindow>();
        }

        private static void EnsureLogDatabase()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(dir);
            var db = Path.Combine(dir, "app-logs.db");
            using var c = new SqliteConnection($"Data Source={db};Mode=ReadWriteCreate;Cache=Shared");
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = """
                CREATE TABLE IF NOT EXISTS system_logging (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    log_date TEXT NOT NULL,
                    log_level TEXT NOT NULL,
                    log_logger TEXT,
                    log_message TEXT,
                    log_exception TEXT
                );
                """;
            cmd.ExecuteNonQuery();
        }
    }

}
