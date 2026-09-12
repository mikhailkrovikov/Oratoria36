using Oratoria.Domain.Devices;
using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum ModuleTableErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неинициализирован")]
        [MechanicErrorMapping(MechanicsErrors.NotInited)]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        Error2_1,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        Error2_2,

        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("Нет ошибок")]
        [MechanicErrorMapping(MechanicsErrors.None)]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        Error2_6,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в откат")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        Error2_3,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в нейтраль")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        Error2_4,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не вышел в обработку")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        Error2_5,
    }
}
