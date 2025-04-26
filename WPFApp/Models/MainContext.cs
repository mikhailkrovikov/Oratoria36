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
        #region Singltone
        private static MainContext _instance;
        public static MainContext Instance => GetInstance();
        private static MainContext GetInstance()
        {
            if (_instance == null)
                _instance = new MainContext();
            return _instance;
        }
        #endregion
        public NetContext Net { get; } = NetContext.Instance;
        public Module2Signals Module2Signals { get; } = new();


    }
}
