using System.Collections.Generic;

namespace Oratoria36.Models
{
    public static class ModulesConfigManager
    {
        private static readonly Dictionary<string, ModuleConfig> Devices = new();

        public static ModuleConfig GetDevice(string key)
        {
            if (!Devices.ContainsKey(key))
            {
                Devices[key] = new ModuleConfig();
            }
            return Devices[key];
        }
    }
}
