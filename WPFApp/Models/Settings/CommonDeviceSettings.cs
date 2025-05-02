using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Settings
{
    public static class CommonDeviceSettings
    { 
        public static Setting<int> ValveTimeForWarning { get; set; } = new Setting<int>("Время открытия/закрытия до предупреждения", "Клапан", 10);
        public static Setting<int> ValveTimeForError { get; set; } = new Setting<int>("Время открытия/закрытия до ошибки", "Клапан", 10);
        public static Setting<int> RRGTimeForWarning { get; set; } = new Setting<int>("Время достижения уставки до предупреждения", "РРГ", 10);
        public static Setting<int> RRGTimeForError { get; set; } = new Setting<int>("Время достижения уставки до ошибки", "РРГ", 10);
        public static Setting<ushort> RRGDifference { get; set; } = new Setting<ushort>("Разница между уставкой и реальным значением", "РРГ", 100);
        public static Setting<int> ManipulatorActionTime {  get; set; } = new Setting<int>("Время движения манипулятора", "Манипулятор", 10);
    }
}
