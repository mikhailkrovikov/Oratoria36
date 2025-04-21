using Oratoria36.Models.Modules;
using Oratoria36.Models.Signals;
using System.Collections.ObjectModel;
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
        Module2Signals _signals;

        public Module2SignalsPage()
        {
            InitializeComponent();
            _vm = new Module2SignalsVM();
            _signals = new Module2Signals();
            DataContext = _vm;
            Initialize(DigitalInputGrid, DigitalOutputGrid, AnalogInputGrid, AnalogOutputGrid);
        }
        private void Initialize(Grid DIGrid, Grid DOGrid, Grid AIGrid, Grid AOGrid)
        {
            ConfigureDISignalGrid(DIGrid, _signals.DISignals.DigitalInputs);
            ConfigureDOSignalGrid(DOGrid, _signals.DOSignals.DigitalOutputs);
            ConfigureAISignalGrid(AIGrid, _signals.AISignals.AnalogInputs);
            ConfigureAOSignalGrid(AOGrid, _signals.AOSignals.AnalogOutputs);
        }
        private static void ConfigureDISignalGrid(Grid grid, ObservableCollection<InputSignal<bool>> signals)
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
        private static void ConfigureDOSignalGrid(Grid grid, ObservableCollection<OutputSignal<bool>> signals)
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
        private static void ConfigureAISignalGrid(Grid grid, ObservableCollection<InputSignal<ushort>> signals)
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

                var valueLabel = new Label()
                {
                    Content = signal.Value,
                    FontWeight = FontWeights.Bold,
                    Style = (Style)Application.Current.FindResource("AnalogBlueValueLabel"),
                };
                Grid.SetRow(valueLabel, rowIndex);
                Grid.SetColumn(valueLabel, 2);
                grid.Children.Add(valueLabel);

                var realValueLabel = new Label()
                {
                    Content = signal.Value,
                    Style = (Style)Application.Current.FindResource("AnalogGreyValueLabel"),
                };
                Grid.SetRow(realValueLabel, rowIndex);
                Grid.SetColumn(realValueLabel, 3);
                grid.Children.Add(realValueLabel);

                rowIndex++;
            }
        }

        private static void ConfigureAOSignalGrid(Grid grid, ObservableCollection<OutputSignal<ushort>> signals)
        {
            int rowIndex = 1;
            foreach (var signal in signals)
            {              
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