using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum OpenableErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не смог открыться")]
        CannotOpen,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не смог закрыться")]
        CannotClose,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("долгое открытие")]
        TooLongOpening,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("долгое закрытие")]
        TooLongClosing,
    }
}