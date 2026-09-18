using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{


    public enum ManipulatorErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        [MechanicErrorMapping(MechanicsErrors.NotInited)]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        IndefinitePos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        UnsertainPos,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        [MechanicErrorMapping(MechanicsErrors.None)]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к ложементу")] 
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        NotComeInPos1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришёл в исходную позицию")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        NotComeInPos2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не опустился к каретке")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        NotComeInPos3,
    }
}
