using Modbus.Device;
using Modbus.Message;
using NLog;
using Oratoria36.Models.Connection;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.Models.Settings;
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
            Net.Module2.Connect(Net.Module2IP);
            Module2Signals = new();

            var thread = new Thread(new ThreadStart(() => ReadInputs()));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ReadInputs()
        {
            Module2DI di = Module2Signals.DISignals;
            Module2AI ai = Module2Signals.AISignals;
            ModbusIpMaster m2Master;


            lock (Locker.Module2PollerLocker)
            {
                if (Net.Module2.IsConnected)
                {
                    m2Master = Net.Module2.Master;

                    var numberOfDevices = m2Master.ReadHoldingRegisters(0x1110, 1);

                    var ids = m2Master.ReadHoldingRegisters(0x110e, (ushort)(numberOfDevices[0] + 1));

                    var name = m2Master.ReadHoldingRegisters(0x1005, 17);


                    string name1 = "";
                    foreach(var cr in name)
                    {
                        var a1 = cr / 256;
                        var a2 = cr % 256;
                        name1 += (char)a1;
                        name1 += (char)a2;
                    }

                    var inputRegSize = m2Master.ReadHoldingRegisters(0x1104, 1);
                    var outputRegSize = m2Master.ReadHoldingRegisters(0x1105, 1);
                    var inputBitsSize = m2Master.ReadHoldingRegisters(0x1108, 1);
                    var outputBitsSize = m2Master.ReadHoldingRegisters(0x1109, 1);


                }
            }


            while (true)
            {
                lock (Locker.Module2PollerLocker)
                {
                    if (Net.Module2.IsConnected)
                    {
                        foreach (var diSignal in di.DigitalInputs)
                        {
                            var a = diSignal.Value;
                        }

                        foreach (var aiSignal in ai.AnalogInputs)
                        {
                            var a = aiSignal.Value;
                        }
                    }
                }

                Thread.Sleep(20);
            }
        }

        #endregion
        public NetContext Net { get; }
        public Module2Signals Module2Signals { get; }


    }
}
