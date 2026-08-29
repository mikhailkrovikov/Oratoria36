using System.ComponentModel;

namespace Oratoria.Application.VacuumModule.DeviceCollection
{
    public enum ManualPumps
    {
        [Description("КН1 (транспорт)")]
        KN1_TM,

        [Description("КН2 (шлюзы)")]
        KN2_Shl,

        [Description("Форнасос")]
        AVR,
    }
}
