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

        [Description("Транспорт: низкий вакуум")]
        TransportLowVacuum,

        [Description("Транспорт: высокий вакуум")]
        TransportHighVacuum,

        [Description("Шлюз 1: низкий вакуум")]
        Gateway1LowVacuum,

        [Description("Шлюз 2: высокий вакуум")]
        Gateway2LowVacuum,

        [Description("КН1 (транспорт): низкий вакуум")]
        KNTransportLowVacuum,

        [Description("КН1 (транспорт): высокий вакуум")]
        KNTransportHighVacuum,

        [Description("КН2 (шлюзы): низкий вакуум")]
        KNGatewaytLowVacuum,

        [Description("КН2 (шлюзы): высокий вакуум")]
        KNGatewayHighVacuum,

        [Description("Трубопровод: низкий вакуум")]
        TrupoprovodLowVacuum,

        [Description("АВР: низкий вакуум")]
        AVRLowVacuum,
    }
}
