using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.PressureSensor
{
    public class HighVacuumSensor : PressureSensor
    {
        public override double CurrentPressure => GetHighVacuumValue(PressureSignal?.Value ?? 0);

        public override PressureStatus State
        {
            get
            {
                var voltage = PressureSignal?.Value ?? 0;
                if (voltage > 9.0)
                    return PressureStatus.Atmosphere;
                if (voltage > 3.0)
                    return PressureStatus.LowVacuum;
                return PressureStatus.HighVacuum;
            }
        }

        public HighVacuumSensor(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory)
            : base(deviceId, signals, loggerFactory) 
        { 
        }

        private static double GetHighVacuumValue(double voltage) 
        {
            if (voltage > 9.00) return 1e5;
            (double voltageOffset, double multiplier) = voltage switch
            {
                > 7.50 => (7.50, 1.0),
                > 6.00 => (6.00, 0.1),
                > 4.50 => (4.50, 0.01),
                > 3.00 => (3.00, 0.001),
                > 1.50 => (1.50, 0.0001),
                _ => (0, 0.00001)
            };
            double pascals = (voltage - voltageOffset + 0.17) / 0.17;
            return pascals * multiplier;
        }
    }
}
