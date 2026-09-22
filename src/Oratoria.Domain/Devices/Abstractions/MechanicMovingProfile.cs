using Oratoria.Domain.Signals;

namespace Oratoria.Domain.Devices.Abstractions
{
    public class MechanicMovingProfile<TErr>(
        OutputSignal<bool> outPos,
        InputSignal<bool> startPos,
        InputSignal<bool> endPos,
        bool revers,
        bool tormos,
        TErr endPosError,
        TErr startPosError) where TErr : Enum
    {
        public OutputSignal<bool> EndPosOutSignal { get; } = outPos;

        public InputSignal<bool> StartPosSignal { get; } = startPos;

        public InputSignal<bool> EndPosSignal { get; } = endPos;

        public bool Revers { get; } = revers;

        public bool Tormos { get; } = tormos;

        public TErr EndPosError { get; } = endPosError;

        public TErr StartPosError { get; } = startPosError;
    }
}
