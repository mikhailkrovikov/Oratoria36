using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2Signals
    {
        NetContext context;
        public Module2DI DISignals;
        public Module2Signals()
        {
            context = NetContext.Instance;
            DISignals = new(context.Module2.Master);
        }
    }
}
