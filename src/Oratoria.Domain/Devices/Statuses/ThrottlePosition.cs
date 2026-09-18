using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ThrottlePosition
    {
        [Description("Неопределенное")]
        [MechanicStatusMapping(MechanicsPositions.Indefinite)]
        Indefinite,

        [Description("Неоднозначное")]
        [MechanicStatusMapping(MechanicsPositions.Uncertain)]
        Uncertain,

        [Description("Переходное")]
        [MechanicStatusMapping(MechanicsPositions.Transition)]
        Transition,

        [Description("Открыт")]
        [MechanicStatusMapping(MechanicsPositions.Position2)]
        Open,

        [Description("Закрыт")]
        [MechanicStatusMapping(MechanicsPositions.Position1)]
        Close,

        [Description("Дросселирование")]
        [MechanicStatusMapping(MechanicsPositions.Position3)]
        Throttling
    }
}
