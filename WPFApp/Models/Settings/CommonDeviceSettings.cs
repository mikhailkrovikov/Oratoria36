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
    }
}
