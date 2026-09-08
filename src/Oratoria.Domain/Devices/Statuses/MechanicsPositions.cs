using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum MechanicsPositions
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Позиция 1")]
        Position1,

        [Description("Позиция 2")]
        Position2,

        [Description("Позиция 3")]
        Position3,

        [Description("Позиция 4")]
        Position4,

        [Description("Позиция 5")]
        Position5,

        [Description("Позиция 6")]
        Position6
    }
}
