using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oratoria36.Models.Connection;

namespace Oratoria36.Models.Modules.Module2
{
    public class Module2Signals
    {
        NetContext _netContext;
        NetConfig _netConfig;
        public Module2DI DISignals { get; }
        public Module2DO DOSignals { get; }
        public Module2AI AISignals { get; }
        public Module2AO AOSignals { get; }
        public Module2Signals()
        {
            _netContext = NetContext.Instance;
            _netConfig = _netContext.Module2;
            DISignals = new(_netConfig);
            DOSignals = new(_netConfig);
            AISignals = new(_netConfig);
            AOSignals = new(_netConfig);
        }
    }
}
