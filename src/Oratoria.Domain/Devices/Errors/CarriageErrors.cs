using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    // пока хз
    public enum CarriageErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        [MechanicErrorMapping(MechanicsErrors.NotInited)]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        IndefinitePosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        UncertainPosition,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        [MechanicErrorMapping(MechanicsErrors.None)]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        NotInStartPosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 1")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        NotCameInPosition1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 2")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        NotCameInPosition2,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 3")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        NotCameInPosition3,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 4")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos4)]
        NotCameInPosition4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 5")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos5)]
        NotCameInPosition5,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не пришел в позицию 6")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos6)]
        NotCameInPosition6,
    }
}
