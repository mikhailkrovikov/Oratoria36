using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ModuleTablePosition
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

        [Description("Исходная")]
        [MechanicStatusMapping(MechanicsPositions.Position1)]
        Home,

        [Description("Откат")]
        [MechanicStatusMapping(MechanicsPositions.Position2)]
        Rollback,

        [Description("Обработка")]
        [MechanicStatusMapping(MechanicsPositions.Position3)]
        Processing,
    }
}
