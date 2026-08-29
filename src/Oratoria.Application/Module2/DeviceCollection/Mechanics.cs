using System.ComponentModel;

namespace Oratoria.Application.Module2.DeviceCollection
{
    public enum  Mechanics
    {
        [Description("Дроссельный затвор")]
        Throttle,

        [Description("Манипулятор")]
        Manipulator,

        [Description("Ложемент")]
        Table,
    }
}
