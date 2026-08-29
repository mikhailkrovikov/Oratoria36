using System.ComponentModel;

namespace Oratoria.Application.VacuumModule.DeviceCollection
{
    public enum Valves
    {
        [Description("ФК модуля 1")]
        FK_M1,

        [Description("ФК модуля 2")]
        FK_M2,

        [Description("ФК модуля 3")]
        FK_M3,

        [Description("ФК модуля 4")]
        FK_M4,

        [Description("ФК АВР")]
        FK_AVR,

        [Description("ФК ОК")]
        FK_OK,

        [Description("ФК АП")]
        FK_AP,

        [Description("ФК КН1")]
        FK_KN1,

        [Description("Затвор КН2")]
        KN2_Zatvor,

        [Description("Затвор КН1")]
        KN_Zatvor_TM,

        [Description("ФК ТрМ")]
        FK_TM,

        [Description("ФК Шл1")]
        FK_Shl1,

        [Description("ФК Шл2")]
        FK_Shl2,

        [Description("ФК Трб")]
        FK_Trb
    }
}
