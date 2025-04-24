using Oratoria36.Models.Connection;
using Oratoria36.Models.Modules.Module2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models
{
    public class MainContext
    {
        private static MainContext _instance;
        public static MainContext Instance => GetInstance();
        public Module2Signals Module2Signals = new();
        private static MainContext GetInstance()
        {
            if (_instance == null)
                _instance = new MainContext();
            return _instance;
        }
    }
}
