using System.ComponentModel;

namespace Oratoria.UI.Controls.Controls.Mnemo
{
    public enum ManipulatorArmPosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Модуль")]
        Module,

        [Description("Исходная")]
        Home,

        [Description("Транспорт")]
        Transport
    }
}
