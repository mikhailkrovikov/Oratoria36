using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum PowerDeviceErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось включить")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен пускатель\n" +
            "- Неисправен БП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotTurnOn,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен пускатель\n" +
            "- Неисправен БП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotTurnOff,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось задать мощность")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен БП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotGetValue,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не удалось сбросить мощность")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен БП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotResetValue,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("неожиданное выключение")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправен БП\n" +
            "- Неисправен пускатель")]
        UnexpectedShutDown,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("перегрев")]
        Overheat,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("перегруз")]
        Overload,
    }
}
