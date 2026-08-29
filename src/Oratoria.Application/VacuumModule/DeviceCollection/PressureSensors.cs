using System.ComponentModel;

namespace Oratoria.Application.VacuumModule.DeviceCollection
{
    public enum PressureSensors
    {
        [Description("Модуль 1: низкий вакуум")]
        Module1LowPressure,

        [Description("Модуль 2: низкий вакуум")]
        Module2LowPressure,

        [Description("Модуль 3: низкий вакуум")]
        Module3LowPressure,

        [Description("Модуль 4: низкий вакуум")]
        Module4LowPressure,
    }
}
