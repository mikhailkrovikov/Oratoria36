using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    // пока хз как назвать
    public enum CarriagePosition
    {
        [Description("Позиция 1")]
        [MechanicStatusMapping(MechanicsPositions.Position1)]
        Position1 = 1,

        [Description("Позиция 2")]
        [MechanicStatusMapping(MechanicsPositions.Position2)]
        Position2 = 2,

        [Description("Позиция 3")]
        [MechanicStatusMapping(MechanicsPositions.Position3)]
        Position3 = 3,

        [Description("Позиция 4")]
        [MechanicStatusMapping(MechanicsPositions.Position4)]
        Position4 = 4,

        [Description("Позиция 5")]
        [MechanicStatusMapping(MechanicsPositions.Position5)]
        Position5 = 5,

        [Description("Позиция 6")]
        [MechanicStatusMapping(MechanicsPositions.Position6)]
        Position6 = 6,

        [Description("Неопределенное")]
        [MechanicStatusMapping(MechanicsPositions.Indefinite)]
        Indefinite = 7,

        [Description("Неоднозначное")]
        [MechanicStatusMapping(MechanicsPositions.Uncertain)]
        Uncertain = 8,

        [Description("Переходное")]
        [MechanicStatusMapping(MechanicsPositions.Transition)]
        Transition = 9,
    }
}
