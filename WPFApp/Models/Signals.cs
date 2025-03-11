using Oratoria36.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oratoria36.Models
{
    public class Signals : INotifyPropertyChanged
    {
        private static ModuleConfig _module = ModulesConfigManager.GetDevice("192.168.0.102");
        private static string _ipAddress = "192.168.0.102";
        public static string IPAddress
        {
            get => _ipAddress;
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    _module = ModulesConfigManager.GetDevice(_ipAddress);
                }
            }
        }

        public static bool IsConnected => _module.IsConnected;

        private static bool[] _digitalInputs;
        private static bool[] _digitalOutputs;
        private static ushort[] _analogInputs;
        private static ushort[] _analogOutputs;

        public static Dictionary<string, ushort> Commands { get; } = new()
        {
            {"Манипулятор из 1 в 2", 100},
            {"Манипулятор из 2 в 1", 101},
            {"Манипулятор из 2 в 3", 102},
            {"Манипулятор из 3 в 2", 103},
            {"Ложемент из 1 в 2", 200},
            {"Ложемент из 2 в 1", 201},
            {"Ложемент из 2 в 3", 202},
            {"Ложемент из 3 в 2", 203},
        };

        public static Dictionary<string, bool> DigitalInputSignals { get; } = new()
        {
            { "Накал есть", false },
            { "Управление ЭВМ", false },
            { "Уровень ЭВМ", false },
            { "Движение БПМ есть", false},
            { "Анод включен", false },
            { "ВЧ включен", false },
            { "ВЧ выключен", false },
            { "УУРГ включено", false },
            { "БПН включен", false },
            { "БПМ1 включен", false },
            { "Перегрев БПМ есть", false },
            { "Перегрузка БПМ есть", false },
            { "БПМ2 включен", false },
            { "Позиция 1", false },
            { "Позиция 2", false },
            { "Позиция 3", false },
            { "БПМ3 включен", false },
            { "Реверс включен", false },
            { "Криогенный насос включен", false },
            { "Натекатель 1 включен", false },
            { "Натекатель 2 включен", false },
            { "БП УОГ включен", false },
            { "ФК-КН ДУ-63 открыт", false },
            { "ФК-КН ДУ-63 закрыт", false },
            { "Заслонка открыта", false },
            { "Заслонка закрыта", false },
            { "ЩЗ открыт", false },
            { "ЩЗ закрыт", false },
            { "Перегрев воды есть", false },
            { "Вода есть", false },
            { "Тормоз включен", false },
            { "Перегруз привода есть", false },
        };

        public static Dictionary<string, bool> DigitalOutputSignals = new()
        {
            { "Вращение магнетронов", false },
            { "Авария вакууметра", false },
            { "Согласование больше", false },
            { "Контроль загаживания вакуума", false },
            { "Обезгаживание вакуума", false },
            { "Термопара включить", false },
            { "Анод включить", false },
            { "Управление ЭВМ включить", false },
            { "Уровень ЭВМ включить", false },
            { "ВЧ выключить", false },
            { "БПН включить", false },
            { "БПМ1 включить", false },
            { "БПМ2 включить", false },
            { "БПМ3 включить", false },
            { "Натекатель 1 включить", false  },
            { "Натекатель 2 включить", false },
            { "БП УОГ включить", false },
            { "УУРГ включить", false },
            { "Привод 3 включить", false },
            { "ФК КН открыть", false },
            { "Заслонка открыть", false },
            { "ЩЗ открыть", false },
            { "Поддув включить (затвор крионасоса)", false },
            { "Привод 1 включить", false },
            { "Привод 2 включить", false },
            { "Привод 4 включить", false },
            { "Позиция 1", false },
            { "Позиция 2", false },
            { "Позиция 3", false },
            { "Реверс включить", false },
            { "Тормоз включить", false },
            { "Криогенный насос включить", false },
        };

        public static Dictionary<string, ushort> AnalogInputSignals = new()
        {
            { "Напряжение БПН", 0 },
            { "Ток БПН", 0 },
            { "Ток БПМ1", 0 },
            { "Напряжение БПМ1", 0 },
            { "Ток БПМ2", 0 },
            { "Напряжение БПМ2", 0 },
            { "Ток БПМ3", 0 },
            { "Напряжение БПМ3", 0 },
            { "Термопара", 0 },
            { "ВИЦБ", 0 },
            { "Расход газа: текущее", 0 },
        };

        public static Dictionary<string, ushort> AnalogOutputSignals = new()
        {
            { "Мощность БПН", 0 },
            { "Мощность БПМ1", 0 },
            { "Мощность БПМ2", 0 },
            { "Мощность БПМ3", 0 },
            { "Управление натекателем", 0 },
            { "Расход газа: уставка", 0 },
        };
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
