using Oratoria.Domain.Devices;
using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum ThrottleErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        IndefinitePosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        UncertainPosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог открыться")]
        CannotOpen,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог закрыться")]
        CannotClose,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог дросселировать")]
        CannotThrottling
    }
}
