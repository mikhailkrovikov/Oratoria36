using DigitalTwin;
using Oratoria.Domain.Signals;
using Oratoria.Domain.Signals.Abstractions;

namespace Oratoria.Application.Strategies
{
    public class DigitalTwinStrategy :
        IInputStrategy<bool>,
        IInputStrategy<double>,
        IOutputStrategy<bool>,
        IOutputStrategy<double>
    {
        private readonly IRegister _model;
        private readonly Dictionary<ushort, InputSignal<bool>> _boolSignals = new();
        private readonly Dictionary<ushort, InputSignal<double>> _doubleSignals = new();

        public DigitalTwinStrategy(IRegister model)
        {
            _model = model;

            _model.BoolInputChanged += (pin, value) =>
            {
                if (_boolSignals.TryGetValue(pin, out var signal))
                    signal.Value = value;
            };

            _model.DoubleInputChanged += (pin, value) =>
            {
                if (_doubleSignals.TryGetValue(pin, out var signal))
                    signal.Value = value;
            };
        }

        public void RegisterSignals(IEnumerable<InputSignal<bool>> signals)
        {
            foreach (var signal in signals)
                _boolSignals[signal.PinNumber] = signal;
        }

        public void RegisterSignals(IEnumerable<InputSignal<double>> signals)
        {
            foreach (var signal in signals)
                _doubleSignals[signal.PinNumber] = signal;
        }

        public bool GetInput(ushort pinNumber) => _model.GetInputBool(pinNumber);
        double IInputStrategy<double>.GetInput(ushort pinNumber) => _model.GetInputDouble(pinNumber);
        public void SetOutput(ushort pinNumber, bool value) => _model.SetOutput(pinNumber, value);
        public void SetOutput(ushort pinNumber, double value) => _model.SetOutput(pinNumber, value);
    }
}
