using System.ComponentModel;

namespace Oratoria.UI.Controls.Controls.Mnemo
{
    public enum TableSlotPosition
    {
        [Description("Неопределенное")]
        Indefinite,

        [Description("Неоднозначное")]
        Uncertain,

        [Description("Переходное")]
        Transition,

        [Description("Исходная")]
        Home,

        [Description("Откат")]
        Rollback,

        [Description("Обработка")]
        Processing
    }
}
