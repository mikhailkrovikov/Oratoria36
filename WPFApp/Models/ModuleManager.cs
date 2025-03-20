using System;
using System.Collections.Generic;
using NLog;

namespace Oratoria36.Models
{
    public class ModuleManager
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, ModuleConfig> _modules;

        public ModuleManager()
        {
            _modules = new Dictionary<string, ModuleConfig>();
        }

        public void AddModule(string ip)
        {
            if (!_modules.ContainsKey(ip))
            {
                var moduleConfig = new ModuleConfig();
                moduleConfig.SetIP(ip);
                _modules.Add(ip, moduleConfig);
                Logger.Info($"Модуль с IP {ip} добавлен и подключен");
            }
            else
            {
                Logger.Warn($"Модуль с IP {ip} уже существует");
            }
        }

        public void RemoveModule(string ip)
        {
            if (_modules.ContainsKey(ip))
            {
                _modules[ip].CloseConnection();
                _modules.Remove(ip);
                Logger.Info($"Модуль с IP {ip} удален");
            }
            else
            {
                Logger.Warn($"Модуль с IP {ip} не найден");
            }
        }

        public ModuleConfig GetModule(string ip)
        {
            if (_modules.ContainsKey(ip))
            {
                return _modules[ip];
            }
            else
            {
                Logger.Warn($"Модуль с IP {ip} не найден");
                throw new KeyNotFoundException($"Модуль с IP {ip} не найден");
            }
        }
    }
}