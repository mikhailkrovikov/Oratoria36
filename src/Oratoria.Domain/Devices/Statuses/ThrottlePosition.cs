using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ThrottlePosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Открыт")]
        Open,

        [Description("Закрыт")]
        Close,

        [Description("Дросселирование")]
        Throttling
    }
}
