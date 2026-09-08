using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    // пока хз как назвать
    public enum CarriagePosition
    {
        [Description("Позиция 1")]
        Position1 = 1,

        [Description("Позиция 2")]
        Position2 = 2,

        [Description("Позиция 3")]
        Position3 = 3,

        [Description("Позиция 4")]
        Position4 = 4,

        [Description("Позиция 5")]
        Position5 = 5,

        [Description("Позиция 6")]
        Position6 = 6,

        [Description("Неопределенное")]
        Indefinite = 7,

        [Description("Неоднозначное")]
        Uncertain = 8,

        [Description("Переходное")]
        Transition = 9,
    }
}
