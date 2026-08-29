using Microsoft.Extensions.Logging;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.Errors;
using Oratoria.Domain.Devices.PressureSensor.PressureSensorAttributes;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oratoria.Domain.Devices.PressureSensor
{
    public abstract class PressureSensor : Device<PressureStatus, PressureErrors>
    {
        public InputSignal<double> PressureSignal { get; protected set; }

        public abstract double CurrentPressure { get; }

        public string CurrentPressureDisplay => FormatScientific(CurrentPressure);

        public abstract override PressureStatus State { get; }

        protected PressureSensor(Enum deviceId, IModuleSignals signals, ILoggerFactory loggerFactory)
            : base(deviceId, loggerFactory)
        {
            PressureSignal = SignalHelper<InputSignal<double>>.GetSignal(deviceId, signals.AISignals, typeof(PressureSensorSignalAttribute<>))!;
            PressureSignal?.OnSignalChanged += _ => OnStateChanged();
        }

        public static string FormatScientific(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                return "1.00 E+5";
            if (value == 0)
                return "0.00 E+0";
            int exponent = (int)Math.Floor(Math.Log10(Math.Abs(value)));
            double mantissa = value / Math.Pow(10, exponent);
            mantissa = Math.Round(mantissa, 2);
            return $"{mantissa:0.00} E{exponent:+0;-#}";
        }
    }
}
