using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2Signals
    {
        NetContext _netContext;
        public Module2DI DISignals;
        public Module2DO DOSignals;
        public Module2Signals()
        {
            _netContext = NetContext.Instance;
            DISignals = new(_netContext.Module2.Master);
            DOSignals = new(_netContext.Module2.Master);
        }
    }
}
