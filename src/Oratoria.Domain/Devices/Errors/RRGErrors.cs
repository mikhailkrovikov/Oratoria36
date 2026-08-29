using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum RRGErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("не удалось достичь уставки")]
        CannotSetCons,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("не удалось сбросить уставку")]
        CannotResetCons,
    }
}
