using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{
    public enum OpenableErrors
    {
        [DeviceErrorCategory(DeviceErrorCategory.None)]
        [Description("нет ошибок")]
        None,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не смог открыться")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправна пневматика\n" +
            "- Неисправны концевики\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotOpen,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("не смог закрыться")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Неисправна пневматика\n" +
            "- Неисправны концевики\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        CannotClose,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("долгое открытие")]
        TooLongOpening,

        [DeviceErrorCategory(DeviceErrorCategory.Warn)]
        [Description("долгое закрытие")]
        TooLongClosing,
    }
}