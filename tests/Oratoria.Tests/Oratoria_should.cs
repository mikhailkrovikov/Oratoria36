using Microsoft.Extensions.Logging;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Tests
{
    [TestFixture]
    public class Oratoria_should
    {
        static IModuleSignals _signals;
        static ILogger logger;
        static ILoggerFactory loggerFactory;

        public Oratoria_should()
        {

        }

        [Test]
        public static void CheckSignalMap()
        {
            //var valve1 = new Valve("ФК-КН", _signals, logger);
        }
    }
}
