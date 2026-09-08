using System.ComponentModel;

namespace Oratoria.Application
{
    public enum ModuleId
    {
        [Description("Транспортный модуль")]
        TransportModule,

        [Description("Вакуумная система")]
        VacuumModule,

        [Description("Модуль 1 трехпозиционного напыления")]
        Module1,

        [Description("Модуль 2 трехпозиционного напыления")]
        Module2,

        [Description("Модуль 3 трехпозиционного напыления")]
        Module3,

        [Description("Модуль 4 трехпозиционного напыления")]
        Module4,
    }
}
