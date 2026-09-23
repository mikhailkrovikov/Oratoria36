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
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен входной аналоговый сигнал\n" +
            "- Нет прохода газа через магистраль\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotSetCons,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("не удалось сбросить уставку")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен входной аналоговый сигнал\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotResetCons,
    }
}
