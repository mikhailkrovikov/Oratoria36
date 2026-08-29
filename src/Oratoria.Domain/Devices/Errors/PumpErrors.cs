using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum PumpErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось включить")]
        CannotTurnOn,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось выключить")]
        CannotTurnOff,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пропал сигнал масляного насоса")]
        UnexpectedOilShutDown,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("пропал сигнал насоса Рутса")]
        UnexpectedRutsShutDown,
    }
}
