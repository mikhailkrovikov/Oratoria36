using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria36.Models.Settings
{
    public class Setting<T>
    {
        readonly string Name;
        readonly string Device;
        public T MaxValue;
        public T MinValue;
        public T Value { get; set; }
        public Setting(string name, string device, T maxValue, T minValue)
        {
            Name = name;
            Device = device;
            MaxValue = maxValue;
            MinValue = minValue;
        }
        public Setting(string name, string device)
        {
            Name = name;
            Device = device;
        }
    }
}
