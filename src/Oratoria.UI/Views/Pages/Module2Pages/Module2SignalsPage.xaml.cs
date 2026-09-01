using System.Windows.Controls;
using Oratoria.Application.Module2.Signals;
using Oratoria.UI.Services;

namespace Oratoria.UI.Views.Pages;

public partial class Module2SignalsPage : Page
{
    public Module2SignalsPage(Module2Signals signals)
    {
        InitializeComponent();
        SignalPageConfig.ConfigureDISignalGrid(DigitalInputGrid, signals.DISignals.DigitalInputs);
        SignalPageConfig.ConfigureDOSignalGrid(DigitalOutputGrid, signals.DOSignals.DigitalOutputs);
        SignalPageConfig.ConfigureAISignalGrid(AnalogInputGrid, signals.AISignals.AnalogInputs);
        SignalPageConfig.ConfigureAOSignalGrid(AnalogOutputGrid, signals.AOSignals.AnalogOutputs);
    }
}
