using Oratoria.Domain.Devices;
using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum MechanicsErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в конечном положении")]
        NotInEndPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        IndefinitePos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        UnsertainPos,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 1")]
        NotComeInPos1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 2")]
        NotComeInPos2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 3")]
        NotComeInPos3,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 4")]
        NotComeInPos4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 5")]
        NotComeInPos5,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 6")]
        NotComeInPos6,
    }
}
