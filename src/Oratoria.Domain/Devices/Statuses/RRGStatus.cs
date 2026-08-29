using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum RRGStatus
    {
        [Description("Закрыт")]
        Close,

        [Description("Дросселирование")]
        Transition,

        [Description("Открыт")]
        Open,
    }
}
