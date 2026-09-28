using System.ComponentModel;

namespace Oratoria.Domain.Devices.Errors
{


    public enum ManipulatorErrors
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

        [DeviceErrorCategory(DeviceErrorCategory.Fatal)]
        [Description("Неопределенное положение")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не приходит ни один позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.IndefinitePos)]
        IndefinitePos,

        [DeviceErrorCategory(DeviceErrorCategory.Fatal)]
        [Description("Неоднозначное положение")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Одновременно приходит более одного сигнала позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.UnsertainPos)]
        UnsertainPos,

        [DeviceErrorCategory(DeviceErrorCategory.Error)]
        [Description("Не в исходном положении")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotInStartPos)]
        NotInStartPos,

        [DeviceErrorCategory(DeviceErrorCategory.Fatal)]
        [Description("Не опустился к ложементу")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos1)]
        NotComeInPos1,

        [DeviceErrorCategory(DeviceErrorCategory.Fatal)]
        [Description("Не пришёл в исходную позицию")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos2)]
        NotComeInPos2,

        [DeviceErrorCategory(DeviceErrorCategory.Fatal)]
        [Description("Не опустился к каретке")]
        [ErrorDescription("Возможные причины ошибки:\n\n" +
            "- Не пришел сигнал целевой позиции\n" +
            "- Неисправен БУП\n" +
            "- Неисправно электрическое подключение к модулю ввода/вывода")]
        [MechanicErrorMapping(MechanicsErrors.NotComeInPos3)]
        NotComeInPos3,
    }
}
