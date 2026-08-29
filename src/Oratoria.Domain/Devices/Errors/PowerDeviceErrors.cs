using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
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

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("неожиданное выключение")]
        UnexpectedShutDown,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("перегрев")]
        Overheat,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("перегруз")]
        Overload,
    }
}
