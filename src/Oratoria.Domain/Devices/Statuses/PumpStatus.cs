using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum PumpStatus
    {
        [Description("Выключен")]
        Off,

        [Description("Промежуточное")]
        Transition,

        [Description("Включен")]
        On
    }
}
