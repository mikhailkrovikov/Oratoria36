using Oratoria.Application.TransportModule.DeviceCollection;
using Oratoria.Domain.Connection;
using Oratoria.Domain.Devices.Shutter.ShutterAttributes;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;
using Oratoria.Domain.Signals.Strategies;
using System.Collections;
using System.Collections.ObjectModel;

namespace Oratoria.Application.TransportModule.Signals
{
    public class TransportDO : IEnumerable<OutputSignal<bool>>
    {
        private readonly IOutputStrategy<bool> _strategy;

        public ObservableCollection<OutputSignal<bool>> DigitalOutputs;

        public OutputSignal<bool> Shl1_Privod1 { get; set; }
        public OutputSignal<bool> Shl1_Privod2 { get; set; }
        public OutputSignal<bool> Shl1_Privod3 { get; set; }
        public OutputSignal<bool> Shl1_Privod4 { get; set; }
        public OutputSignal<bool> Shl1_Pos1 { get; set; }
        public OutputSignal<bool> Shl1_Pos2 { get; set; }
        public OutputSignal<bool> Shl1_Pos3 { get; set; }
        public OutputSignal<bool> Shl1_Pos4 { get; set; }
        public OutputSignal<bool> Shl2_Privod1 { get; set; }
        public OutputSignal<bool> Shl2_Privod2 { get; set; }
        public OutputSignal<bool> Shl2_Privod3 { get; set; }
        public OutputSignal<bool> Shl2_Privod4 { get; set; }
        public OutputSignal<bool> Shl2_Pos1 { get; set; }
        public OutputSignal<bool> Shl2_Pos2 { get; set; }
        public OutputSignal<bool> Shl2_Pos3 { get; set; }
        public OutputSignal<bool> Shl2_Pos4 { get; set; }
        public OutputSignal<bool> Shl1_Revers { get; set; }
        public OutputSignal<bool> Shl1_Tormos { get; set; }


        [ShutterOpenSignal<Shutters>(Shutters.Shl1Shutter)]
        public OutputSignal<bool> Shl1_Zatvor { get; set; }

        public OutputSignal<bool> Shl1_Podduv { get; set; }
        public OutputSignal<bool> Shl1_Napusk { get; set; }
        public OutputSignal<bool> Shl1_UURG1 { get; set; }
        public OutputSignal<bool> Shl2_Revers { get; set; }
        public OutputSignal<bool> Shl2_Tormos { get; set; }


        [ShutterOpenSignal<Shutters>(Shutters.Shl2Shutter)]
        public OutputSignal<bool> Shl2_Zatvor { get; set; }

        public OutputSignal<bool> Shl2_Podduv { get; set; }
        public OutputSignal<bool> Shl2_Napusk { get; set; }
        public OutputSignal<bool> Shl2_UURG2 { get; set; }
        public OutputSignal<bool> Position5 { get; set; }
        public OutputSignal<bool> Position6 { get; set; }

        public TransportDO(ModbusTCPConfig netConfig, IOutputStrategy<bool> strategy)
        {
#if RELEASE
            _strategy = new DOModbusStrategy(netConfig);
#else
            _strategy = strategy;
#endif

            Shl1_Privod1 = new OutputSignal<bool>("Шлюз 1: привод 1 включить", 0, _strategy);
            Shl1_Privod2 = new OutputSignal<bool>("Шлюз 1: привод 2 включить", 1, _strategy);
            Shl1_Privod3 = new OutputSignal<bool>("Шлюз 1: привод 3 включить", 2, _strategy);
            Shl1_Privod4 = new OutputSignal<bool>("Шлюз 1: привод 4 включить", 3, _strategy);
            Shl1_Pos1 = new OutputSignal<bool>("Шлюз 1: позиция 1", 4, _strategy);
            Shl1_Pos2 = new OutputSignal<bool>("Шлюз 1: позиция 2", 5, _strategy);
            Shl1_Pos3 = new OutputSignal<bool>("Шлюз 1: позиция 3", 6, _strategy);
            Shl1_Pos4 = new OutputSignal<bool>("Шлюз 1: позиция 4", 7, _strategy);
            Shl2_Privod1 = new OutputSignal<bool>("Шлюз 2: привод 1 включить", 8, _strategy);
            Shl2_Privod2 = new OutputSignal<bool>("Шлюз 2: привод 2 включить", 9, _strategy);
            Shl2_Privod3 = new OutputSignal<bool>("Шлюз 2: привод 3 включить", 10, _strategy);
            Shl2_Privod4 = new OutputSignal<bool>("Шлюз 2: привод 4 включить", 11, _strategy);
            Shl2_Pos1 = new OutputSignal<bool>("Шлюз 2: позиция 1", 12, _strategy);
            Shl2_Pos2 = new OutputSignal<bool>("Шлюз 2: позиция 2", 13, _strategy);
            Shl2_Pos3 = new OutputSignal<bool>("Шлюз 2: позиция 3", 14, _strategy);
            Shl2_Pos4 = new OutputSignal<bool>("Шлюз 2: позиция 4", 15, _strategy);
            Shl1_Revers = new OutputSignal<bool>("Шлюз 1: реверс включить", 16, _strategy);
            Shl1_Tormos = new OutputSignal<bool>("Шлюз 1: тормоз включить", 17, _strategy);
            Shl1_Zatvor = new OutputSignal<bool>("Шлюз 1: ЩЗ открыть", 18, _strategy);
            Shl1_Podduv = new OutputSignal<bool>("Шлюз 1: поддув включить", 19, _strategy);
            Shl1_Napusk = new OutputSignal<bool>("Шлюз 1: напуск", 20, _strategy);
            Shl1_UURG1 = new OutputSignal<bool>("Шлюз 1: УУРГ-1 включить", 21, _strategy);
            Shl2_Revers = new OutputSignal<bool>("Шлюз 2: реверс включить", 22, _strategy);
            Shl2_Tormos = new OutputSignal<bool>("Шлюз 2: тормоз включить", 23, _strategy);
            Shl2_Zatvor = new OutputSignal<bool>("Шлюз 2: ЩЗ открыть", 24, _strategy);
            Shl2_Podduv = new OutputSignal<bool>("Шлюз 2: поддув включить", 25, _strategy);
            Shl2_Napusk = new OutputSignal<bool>("Шлюз 2: напуск", 26, _strategy);
            Shl2_UURG2 = new OutputSignal<bool>("Шлюз 2: УУРГ-2 включить", 27, _strategy);
            Position5 = new OutputSignal<bool>("Позиция 5", 28, _strategy);
            Position6 = new OutputSignal<bool>("Позиция 6", 29, _strategy);

            DigitalOutputs =
            [
                Shl1_Privod1,
                Shl1_Privod2,
                Shl1_Privod3,
                Shl1_Privod4,
                Shl1_Pos1,
                Shl1_Pos2,
                Shl1_Pos3,
                Shl1_Pos4,
                Shl2_Privod1,
                Shl2_Privod2,
                Shl2_Privod3,
                Shl2_Privod4,
                Shl2_Pos1,
                Shl2_Pos2,
                Shl2_Pos3,
                Shl2_Pos4,
                Shl1_Revers,
                Shl1_Tormos,
                Shl1_Zatvor,
                Shl1_Podduv,
                Shl1_Napusk,
                Shl1_UURG1,
                Shl2_Revers,
                Shl2_Tormos,
                Shl2_Zatvor,
                Shl2_Podduv,
                Shl2_Napusk,
                Shl2_UURG2,
                Position5,
                Position6,
            ];
        }

        public IEnumerator<OutputSignal<bool>> GetEnumerator()
        {
            return DigitalOutputs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
