using Modbus.Message;
using NLog;
using Oratoria36.Models.Connection;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Oratoria36.Models
{
    public class MainContext
    {
        #region Singltone
        private static MainContext _instance;
        public static MainContext Instance => GetInstance();
        private static readonly Logger _logger = LogManager.GetLogger("");

        private static MainContext GetInstance()
        {
            if (_instance == null)
                _instance = new MainContext();
            return _instance;
        }

        private MainContext()
        {
            Net = NetContext.Instance;

            Net.Module2.Connect("192.168.0.100");
            Module2Signals = new();

            var thread = new Thread(new ThreadStart(() => ReadInputs()));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ReadInputs()
        {
            Module2DI di = Module2Signals.DISignals;


            while (true)
            {
                if (Net.Module2.IsConnected)
                {
                    foreach (var signal in di.DigitalInputs)
                    {
                        var a = signal.Value;
                    }
                }

                //_logger.Info("Чтение заваршено");


                Thread.Sleep(10);
            }
        }

        private void DoSmt()
        {

        }

        #endregion
        public NetContext Net { get; }
        public Module2Signals Module2Signals { get; }


    }
}
