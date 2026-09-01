using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Domain.Devices.PressureSensor
{
    public class LowVacuumSensor : PressureSensor
    {
        public override double CurrentPressure => GetLowVacuumValue(PressureSignal?.Value ?? 0);

        public override PressureStatus State
        {
            get
            {
                if (CurrentPressure >= 1e3)
                    return PressureStatus.Atmosphere;
                return PressureStatus.LowVacuum;
            }
        }

        public LowVacuumSensor(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings)
            : base(deviceId, signals, loggerFactory, settings) 
        { 
        }

        private static double GetLowVacuumValue(double voltage) 
        {
            voltage = Math.Max(voltage, 0.4);
            (double slope, double intercept) = voltage switch
            {
                >= 9.4 => (2.17, -16.7),
                >= 9.1 => (1.19, -7.48),
                >= 3.5 => (0.293, 0.67),
                >= 2.1 => (0.5, -0.05),
                _ => (1.0, -1.1)
            };

            return Math.Pow(10, slope * voltage + intercept);
        }
    }
}
