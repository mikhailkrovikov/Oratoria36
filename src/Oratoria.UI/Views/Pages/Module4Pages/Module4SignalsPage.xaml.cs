using System.Windows.Controls;
using Oratoria.Application.Module4.Signals;
using Oratoria.UI.Services;

namespace Oratoria.UI.Views.Pages;

public partial class Module4SignalsPage : Page
{
    public Module4SignalsPage(Module4Signals signals)
    {
        InitializeComponent();
        SignalPageConfig.ConfigureDISignalGrid(DigitalInputGrid, signals.DISignals.DigitalInputs);
        SignalPageConfig.ConfigureDOSignalGrid(DigitalOutputGrid, signals.DOSignals.DigitalOutputs);
        SignalPageConfig.ConfigureAISignalGrid(AnalogInputGrid, signals.AISignals.AnalogInputs);
        SignalPageConfig.ConfigureAOSignalGrid(AnalogOutputGrid, signals.AOSignals.AnalogOutputs);
    }
}
