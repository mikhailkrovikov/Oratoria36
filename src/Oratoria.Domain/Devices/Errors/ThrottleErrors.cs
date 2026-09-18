using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum ThrottleErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        [MechanicErrorMapping(MechanicsErrors.None)]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        [MechanicErrorMapping(MechanicsErrors.NotInited)]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        IndefinitePosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        UncertainPosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог открыться")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        CannotOpen,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог закрыться")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        CannotClose,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог дросселировать")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        CannotThrottling
    }
}
