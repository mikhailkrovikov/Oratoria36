using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Oratoria36.Models.Signals
{
    public abstract class Signal : INotifyPropertyChanged
    {
        protected static readonly Logger _logger = LogManager.GetLogger("Signalold");

        public ushort Channel { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsValid { get; protected set; } = true;

        protected ModuleConfig _module;

        protected Signal(string name, ushort channel, ModuleConfig module, string description = "")
        {
            Name = name;
            Channel = channel;
            Description = description ?? string.Empty;
            _module = module;

            module?.RegisterSignal(this);
        }

        public abstract Task<bool> ReadValueAsync(ModuleConfig module);
        public abstract Task<bool> WriteValueAsync(ModuleConfig module);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public abstract class Signal<T> : Signal
    {
        public event Action<T> OnSignalChanged;

        private T _value;

        public virtual T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));

                    OnSignalChanged?.Invoke(value);
                    _logger.Info($"Сигнал {Name} изменил значение на {value}");

                    if (_module != null && _module.IsConnected)
                    {
                        Task.Run(async () => await WriteValueAsync(_module));
                    }
                }
            }
        }

        protected Signal(string name, ushort channel, ModuleConfig module, string description = "")
            : base(name, channel, module, description)
        {
        }
    }
}
