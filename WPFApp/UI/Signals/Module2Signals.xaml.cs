using Oratoria36.Models.Modules;
using Oratoria36.Models.Signals;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Oratoria36.UI.Signals
{
    public partial class Module2SignalsPage : Page
    {
        Module2SignalsVM _vm;
        Module2Signals _signals = new Module2Signals();

        public Module2SignalsPage()
        {
            InitializeComponent();
            _vm = new Module2SignalsVM();
            DataContext = _vm;
            Initialize(DigitalInputGrid, DigitalOutputGrid);
        }
        private void Initialize(Grid inputGrid, Grid outputGrid)
        {
            ConfigureSignalGrid(inputGrid, _signals.DISignals.DigitalInputs);
        }
        private void ConfigureSignalGrid(Grid grid, List<InputSignal<bool>> signals)
        {
            int rowIndex = 1;
            foreach (var signal in signals)
            {
                var pinLabel = new Label()
                {
                    Content = signal.PinNumber,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(pinLabel, rowIndex);
                Grid.SetColumn(pinLabel, 0);
                grid.Children.Add(pinLabel);


                var nameLabel = new Label()
                {
                    Content = signal.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(nameLabel, rowIndex);
                Grid.SetColumn(nameLabel, 1);
                grid.Children.Add(nameLabel);

                var valueCheckBox = new CheckBox()
                {
                    Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                    IsChecked = signal.Value
                };
                Grid.SetRow(valueCheckBox, rowIndex);
                Grid.SetColumn(valueCheckBox, 2);
                grid.Children.Add(valueCheckBox);

                rowIndex++;
            }
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