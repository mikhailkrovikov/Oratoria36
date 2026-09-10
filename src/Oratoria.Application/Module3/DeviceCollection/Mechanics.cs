using System.ComponentModel;

namespace Oratoria.Application.Module3.DeviceCollection
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
