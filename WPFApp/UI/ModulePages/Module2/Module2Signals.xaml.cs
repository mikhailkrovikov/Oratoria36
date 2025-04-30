using Oratoria36.Models;
using Oratoria36.Models.Connection;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.UI.Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Oratoria36.UI.Signals
{
    public partial class Module2SignalsPage : Page,  ISignalPageConfig
    {
        public NetConfig NetConfig => NetContext.Instance.Module2;
        Module2SignalsVM _vm;
        Module2Signals _signals;

        public Module2SignalsPage()
        {
            InitializeComponent();
            _vm = new Module2SignalsVM();
            _signals = MainContext.Instance.Module2Signals;
            DataContext = _vm;
            Initialize();
        }

        private void Initialize()
        {
            ((ISignalPageConfig)this).ConfigureDISignalGrid(DigitalInputGrid, _signals.DISignals.DigitalInputs);
            ((ISignalPageConfig)this).ConfigureDOSignalGrid(DigitalOutputGrid, _signals.DOSignals.DigitalOutputs);
            ((ISignalPageConfig)this).ConfigureAISignalGrid(AnalogInputGrid, _signals.AISignals.AnalogInputs);
            ((ISignalPageConfig)this).ConfigureAOSignalGrid(AnalogOutputGrid, _signals.AOSignals.AnalogOutputs);
        }
    }

    public class Module2SignalsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}