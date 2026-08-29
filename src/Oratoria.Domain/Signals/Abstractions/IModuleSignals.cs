namespace Oratoria.Domain.Signals.Abstractions
{
    public interface IModuleSignals
    //<TDI, TDO, TAI, TAO> where TDI : IEnumerable<InputSignal<bool>>
    //                                                    where TDO : IEnumerable<OutputSignal<bool>>
    //                                                    where TAI : IEnumerable<InputSignal<double>>
    //                                                    where TAO : IEnumerable<OutputSignal<double>> 
    {
        public IEnumerable<InputSignal<bool>> DISignals { get; }
        public IEnumerable<OutputSignal<bool>> DOSignals { get; }
        public IEnumerable<InputSignal<double>> AISignals { get; }
        public IEnumerable<OutputSignal<double>> AOSignals { get; }
    }
}
