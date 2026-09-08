using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    // пока хз
    public enum CarriageErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        IndefinitePosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        UncertainPosition,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        NotInStartPosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 1")]
        NotCameInPosition1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 2")]
        NotCameInPosition2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 3")]
        NotCameInPosition3,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 4")]
        NotCameInPosition4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 5")]
        NotCameInPosition5,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 6")]
        NotCameInPosition6,
    }
}
