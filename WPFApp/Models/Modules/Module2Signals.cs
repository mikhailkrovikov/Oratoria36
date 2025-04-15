using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Modules
{
    public class Module2Signals
    {
        MainContext context;
        public Module2DI DISignals;
        public Module2Signals()
        {
            context = MainContext.Instance;
            DISignals = new(context.Module2.Master);
        }
    }
}
