using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ModuleTablePosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Исходная")]
        Home,

        [Description("Откат")]
        Rollback,

        [Description("Обработка")]
        Processing,
    }
}
