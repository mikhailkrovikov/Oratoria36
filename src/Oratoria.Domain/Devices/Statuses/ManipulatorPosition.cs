using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ManipulatorPosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Модуль")]
        Module,

        [Description("Исходная")]
        Home,

        [Description("Транспорт")]
        Transport
    }
}
