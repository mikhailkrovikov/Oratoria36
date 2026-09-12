using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum ManipulatorPosition
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

        [Description("Модуль")]
        [MechanicStatusMapping(MechanicsPositions.Position1)]
        Module,

        [Description("Исходная")]
        [MechanicStatusMapping(MechanicsPositions.Position2)]
        Home,

        [Description("Транспорт")]
        [MechanicStatusMapping(MechanicsPositions.Position3)]
        Transport
    }
}
