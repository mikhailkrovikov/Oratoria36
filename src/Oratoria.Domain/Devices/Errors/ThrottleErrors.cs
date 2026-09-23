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
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не выполнена предварительная инициализация\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotInited)]
        NotInited,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Позиция отправления не соответствует текущей\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неопределенное положение")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не приходит ни один позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        IndefinitePosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Неоднозначное положение")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Одновременно приходит более одного сигнала позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        UncertainPosition,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог открыться")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        CannotOpen,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог закрыться")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        CannotClose,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не смог дросселировать")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        CannotThrottling
    }
}
