using System.ComponentModel;

namespace Oratoria.UI.Controls.Controls.Mnemo
{
    public enum ThrottleFlapPosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Открыт")]
        Open,

        [Description("Закрыт")]
        Close,

        [Description("Дросселирование")]
        Throttling
    }
}
