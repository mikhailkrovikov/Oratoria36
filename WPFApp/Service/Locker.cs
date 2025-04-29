using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Service
{
    public static class Locker
    {
        public static object Module1PollerLocker = new object();
        public static object Module2PollerLocker = new object();
        public static object Module3PollerLocker = new object();
        public static object Module4PollerLocker = new object();
        public static object TransportPollerLocker = new object();
    }
}