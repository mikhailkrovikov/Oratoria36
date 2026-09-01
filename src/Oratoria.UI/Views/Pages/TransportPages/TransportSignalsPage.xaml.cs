using System.Windows.Controls;
using Oratoria.Application.TransportModule.Signals;
using Oratoria.UI.Services;

namespace Oratoria.UI.Views.Pages;

public partial class TransportSignalsPage : Page
{
    public TransportSignalsPage(TransportSignals signals)
    {
        InitializeComponent();
        SignalPageConfig.ConfigureDISignalGrid(DigitalInputGrid, signals.DISignals.DigitalInputs);
        SignalPageConfig.ConfigureDOSignalGrid(DigitalOutputGrid, signals.DOSignals.DigitalOutputs);
        SignalPageConfig.ConfigureAISignalGrid(AnalogInputGrid, signals.AISignals.AnalogInputs);
        SignalPageConfig.ConfigureAOSignalGrid(AnalogOutputGrid, signals.AOSignals.AnalogOutputs);
    }
}
