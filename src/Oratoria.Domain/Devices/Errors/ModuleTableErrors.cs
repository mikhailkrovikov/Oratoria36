using Oratoria.Domain.Devices;
using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum ModuleTableErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        Error2_1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        Error2_2,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        Error2_6,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в откат")]
        Error2_3,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в нейтраль")]
        Error2_4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в обработку")]
        Error2_5,
    }
}
