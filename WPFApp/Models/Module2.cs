using Oratoria36.Models.Signals;

namespace Oratoria36.Models
{
    public static class Module2
    {

        private static ModuleConfig _config;

        public static InputSignal Input1 { get; } = new InputSignal("Датчик вакуума", 0);
        public static InputSignal Input2 { get; } = new InputSignal("Датчик температуры", 1);
        public static InputSignal Input3 { get; } = new InputSignal("Датчик положения", 2);
        public static InputSignal Input4 { get; } = new InputSignal("Датчик двери", 3);

        public static OutputSignal Output1 { get; } = new OutputSignal("Нагреватель", 0);
        public static OutputSignal Output2 { get; } = new OutputSignal("Вакуумный насос", 1);
        public static OutputSignal Output3 { get; } = new OutputSignal("Клапан сброса", 2);
        public static OutputSignal Output4 { get; } = new OutputSignal("Блокировка двери", 3);

        public static void Initialize(ModuleConfig config)
        {
            _config = config;
        }

        public static ModuleConfig Config => _config;

        public static bool IsConnected => _config?.IsConnected ?? false;
    }
}
