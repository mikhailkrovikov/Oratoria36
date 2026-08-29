using System.ComponentModel;

namespace Oratoria.Domain.Devices.Statuses
{
    public enum PowerDeviceErrors
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
        [Description("не удалось задать мощность")]
        CannotGetValue,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось сбросить мощность")]
        CannotResetValue,
    }
}
