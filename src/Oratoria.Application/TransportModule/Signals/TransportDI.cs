using Oratoria.Domain.Connection;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.TransportModule.Signals
{
    public class TransportDI : IEnumerable<InputSignal<bool>>
    {
        private IInputStrategy<bool> _strategy;

        public ObservableCollection<InputSignal<bool>> DigitalInputs;

        public InputSignal<bool> Shl1_Pos1 { get; set; }
        public InputSignal<bool> Shl1_Pos2 { get; set; }
        public InputSignal<bool> Shl1_Pos3 { get; set; }
        public InputSignal<bool> Shl1_Pos4 { get; set; }
        public InputSignal<bool> Shl1_Revers { get; set; }
        public InputSignal<bool> Shl1_Tormos { get; set; }
        public InputSignal<bool> Shl1_Peregruz { get; set; }
        public InputSignal<bool> Shl1_UURG1 { get; set; }
        public InputSignal<bool> Shl2_Pos1 { get; set; }
        public InputSignal<bool> Shl2_Pos2 { get; set; }
        public InputSignal<bool> Shl2_Pos3 { get; set; }
        public InputSignal<bool> Shl2_Pos4 { get; set; }
        public InputSignal<bool> Shl2_Revers { get; set; }
        public InputSignal<bool> Shl2_Tormos { get; set; }
        public InputSignal<bool> Shl2_Peregruz { get; set; }
        public InputSignal<bool> Shl2_UURG2 { get; set; }
        public InputSignal<bool> Zatvor_Shl1_Open { get; set; }
        public InputSignal<bool> Zatvor_Shl1_Closed { get; set; }
        public InputSignal<bool> Door_Shl1_Closed { get; set; }
        public InputSignal<bool> Zatvor_Shl2_Open { get; set; }
        public InputSignal<bool> Zatvor_Shl2_Closed { get; set; }
        public InputSignal<bool> Door_Shl2_Closed { get; set; }
        public InputSignal<bool> Position5 { get; set; }
        public InputSignal<bool> Position6 { get; set; }

        public TransportDI(ModbusTCPConfig netConfig, IInputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DIModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Shl1_Pos1 = new InputSignal<bool>("Шлюз 1: позиция 1", 0, _strategy);
            Shl1_Pos2 = new InputSignal<bool>("Шлюз 1: позиция 2", 1, _strategy);
            Shl1_Pos3 = new InputSignal<bool>("Шлюз 1: позиция 3", 2, _strategy);
            Shl1_Pos4 = new InputSignal<bool>("Шлюз 1: позиция 4", 3, _strategy);
            Shl1_Revers = new InputSignal<bool>("Шлюз 1: реверс включен", 4, _strategy);
            Shl1_Tormos = new InputSignal<bool>("Шлюз 1: тормоз включен", 5, _strategy);
            Shl1_Peregruz = new InputSignal<bool>("Шлюз 1: перегруз привода", 6, _strategy);
            Shl1_UURG1 = new InputSignal<bool>("Шлюз 1: УУРГ-1 включен", 7, _strategy);
            Shl2_Pos1 = new InputSignal<bool>("Шлюз 2: позиция 1", 8, _strategy);
            Shl2_Pos2 = new InputSignal<bool>("Шлюз 2: позиция 2", 8, _strategy);
            Shl2_Pos3 = new InputSignal<bool>("Шлюз 2: позиция 3", 10, _strategy);
            Shl2_Pos4 = new InputSignal<bool>("Шлюз 2: позиция 4", 11, _strategy);
            Shl2_Revers = new InputSignal<bool>("Шлюз 2: реверс включен", 12, _strategy);
            Shl2_Tormos = new InputSignal<bool>("Шлюз 2: тормоз включен", 13, _strategy);
            Shl2_Peregruz = new InputSignal<bool>("Шлюз 2: перегруз привода", 14, _strategy);
            Shl2_UURG2 = new InputSignal<bool>("Шлюз 2: УУРГ-2 включен", 15, _strategy);
            Zatvor_Shl1_Open = new InputSignal<bool>("ЩЗ шлюза 1 открыт", 16, _strategy);
            Zatvor_Shl1_Closed = new InputSignal<bool>("ЩЗ шлюза 1 закрыт", 17, _strategy);
            Door_Shl1_Closed = new InputSignal<bool>("Дверь шлюза 1 закрыта", 18, _strategy);
            Zatvor_Shl2_Open = new InputSignal<bool>("ЩЗ шлюза 2 открыт", 19, _strategy);
            Zatvor_Shl2_Closed = new InputSignal<bool>("ЩЗ шлюза 2 закрыт", 20, _strategy);
            Door_Shl2_Closed = new InputSignal<bool>("Дверь шлюза 2 закрыта", 21, _strategy);
            Position5 = new InputSignal<bool>("Позиция 5", 22, _strategy);
            Position6 = new InputSignal<bool>("Позиция 6", 23, _strategy);

            DigitalInputs =
            [
                Shl1_Pos1,
                Shl1_Pos2,
                Shl1_Pos3,
                Shl1_Pos4,
                Shl1_Revers,
                Shl1_Tormos,
                Shl1_Peregruz,
                Shl1_UURG1,
                Shl2_Pos1,
                Shl2_Pos2,
                Shl2_Pos3,
                Shl2_Pos4,
                Shl2_Revers,
                Shl2_Tormos,
                Shl2_Peregruz,
                Shl2_UURG2,
                Zatvor_Shl1_Open,
                Zatvor_Shl1_Closed,
                Door_Shl1_Closed ,
                Zatvor_Shl2_Open,
                Zatvor_Shl2_Closed,
                Door_Shl2_Closed,
                Position5,
                Position6,
            ];
        }

        public IEnumerator<InputSignal<bool>> GetEnumerator()
        {
            return DigitalInputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
