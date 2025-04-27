using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Settings
{
    public static class CommonDeviceSettings
    {
       
        public static Setting<int> ValveTimeForWarning { get; set; } = new Setting<int>("Время открытия/закрытия до предупреждения", "Клапан");
        public static Setting<int> ValveTimeForError { get; set; } = new Setting<int>("Время открытия/закрытия до ошибки", "Клапан");
        public static Setting<int> RRGTimeForWarning { get; set; } = new Setting<int>("Время достижения уставки до предупреждения", "РРГ");
        public static Setting<int> RRGTimeForError { get; set; } = new Setting<int>("Время достижения уставки до ошибки", "РРГ");
        public static Setting<ushort> RRGDifference { get; set; } = new Setting<ushort>("Разница между уставкой и реальным значением", "РРГ");
    }
}
