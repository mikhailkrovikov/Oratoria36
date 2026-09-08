using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum OpenableStatus
    {
        [Description("Закрыт")]
        Close,

        [Description("Промежуточное")]
        Transition,

        [Description("Открыт")]
        Open
    }
}