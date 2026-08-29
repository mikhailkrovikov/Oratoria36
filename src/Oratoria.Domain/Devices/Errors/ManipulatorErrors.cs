using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{


    public enum ManipulatorErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        Error1_2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        Error1_3,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        Error1_1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к ложементу")] 
        Error1_4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не поднялся от ложемента к исходному")]
        Error1_5,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к каретке")]
        Error1_6,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не поднялся от каретки к исходному")]
        Error1_7,
    }
}
