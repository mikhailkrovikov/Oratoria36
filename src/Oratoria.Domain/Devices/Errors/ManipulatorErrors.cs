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
        IndefinitePos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        UnsertainPos,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к ложементу")] 
        NotComeInPos1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришёл в исходную позицию")]
        NotComeInPos2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к каретке")]
        NotComeInPos3,

        //[DeviceErrorCategory(DeviceErrorCategory.Error)]
        //[Description("Не поднялся от каретки к исходному")]
        //Error1_7,
    }
}
