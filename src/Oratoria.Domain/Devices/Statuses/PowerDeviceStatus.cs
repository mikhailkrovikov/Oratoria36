using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum PowerDeviceStatus
    {
        [Description("Выключен")]
        Off,

        [Description("Промежуточное")]
        Transition,

        [Description("Включен")]
        On
    }
}
