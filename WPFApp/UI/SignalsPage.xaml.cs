using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Modbus.Extensions.Enron;
using NLog;
using Oratoria36.Models;


namespace Oratoria36.UI
{
    public partial class SignalsPage : Page
    {
       
        private readonly SignalsVM _signalsVM;
        
        public SignalsPage()
        {
            InitializeComponent();
            
            _signalsVM = new SignalsVM(this);
            DataContext = _signalsVM;
        }
    }
    public class SignalsVM : INotifyPropertyChanged
    {
        private readonly ModuleConfig _modbusDevice;
        public SignalsVM(SignalsPage page)
        {
            _modbusDevice = ModulesConfigManager.GetDevice("192.168.0.102");
            
            ModbusCommandsNameConfig(Signals.Commands, page.CommandGrid);
            SignalsNameConfig(Signals.Commands, page.CommandGrid);
            SignalsNameConfig(Signals.DigitalInputSignals, page.DigitalInputGrid);
            SignalsNameConfig(Signals.DigitalOutputSignals, page.DigitalOutputGrid);
            SignalsNameConfig(Signals.AnalogInputSignals, page.AnalogInputGrid);
            SignalsNameConfig(Signals.AnalogOutputSignals, page.AnalogOutputGrid);

            ModbsusCommandsButtonsConfig(Signals.Commands, _modbusDevice.IsConnected, page.CommandGrid);
            SignalsValueConfig(Signals.DigitalInputSignals, page.DigitalInputGrid);
            SignalsValueConfig(Signals.DigitalOutputSignals, page.DigitalOutputGrid);
            SignalsValueConfig(Signals.AnalogInputSignals, page.AnalogInputGrid);
            SignalsValueConfig(Signals.AnalogOutputSignals, page.AnalogOutputGrid);

            ConvertedValue(Signals.AnalogInputSignals.Values.ToList(), page.AnalogInputGrid);
            ConvertedValue(Signals.AnalogOutputSignals.Values.ToList(), page.AnalogOutputGrid);
        }

        private void ModbsusCommandsButtonsConfig(Dictionary<string, ushort> commands, bool isConnected, Grid grid)
        {
            int rowIndex = 2;
            foreach (var command in commands)
            {
                Button button = new Button
                {
                    IsEnabled = isConnected,
                    Content = "Выполнить",
                    Background = new SolidColorBrush(Color.FromRgb(198, 198, 198)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)),
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                if (!isConnected)
                {
                    button.Content = "Нет связи";
                    button.Background = new SolidColorBrush(Color.FromRgb(63, 63, 63));
                    button.BorderBrush = new SolidColorBrush(Color.FromRgb(192, 192, 192));
                    button.Foreground = new SolidColorBrush(Color.FromRgb(192, 192, 192));
                }
                button.Click += (sender, e) => _modbusDevice.WriteSingleRegister(40, command.Value);
                Grid.SetRow(button, rowIndex);
                Grid.SetColumn(button, 2);
                grid.Children.Add(button);
                rowIndex++;
            } 
        }

        private void ModbusCommandsNameConfig<T>(Dictionary<string, T> commands, Grid grid)
        {
            int rowIndex = 2;
            foreach (var command in commands)
            {
                Label label = new Label
                {
                    Content = command.Value,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    BorderThickness = new Thickness(0, 1, 1, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(192, 192, 192))
                };
                if (rowIndex == commands.Count + 1)
                {
                    label.BorderThickness = new Thickness(0, 1, 1, 1);
                }
                Grid.SetRow(label, rowIndex);
                Grid.SetColumn(label, 1);
                grid.Children.Add(label);
                rowIndex++;

            }
        }

        private void SignalsNameConfig<T>(Dictionary<string, T> inputs, Grid grid)
        {
            int rowIndex = 2;
            foreach (var signal in inputs)
            {
                Label label = new Label
                {
                    Content = signal.Key,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    BorderThickness = new Thickness(1, 1, 0, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(192, 192, 192))
                };
                if (rowIndex == inputs.Count + 1)
                {
                    label.BorderThickness = new Thickness(1, 1, 0, 1);
                }
                Grid.SetRow(label, rowIndex);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);
                rowIndex++;

            }
        }
        private void SignalsValueConfig<T>(Dictionary<string, T> inputs, Grid grid)
        {
            int rowIndex = 2;
            foreach (var signal in inputs)
            {
                object contentValue = signal.Value;
                if (signal.Value is bool boolValue)
                {
                    if (boolValue)
                    {
                        contentValue = "1";
                    }
                    else
                    {
                        contentValue = "0";
                    }
                }

                Label label = new Label()
                {
                    Content = contentValue,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    BorderThickness = new Thickness(0, 1, 1, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(192, 192, 192)),
                    HorizontalContentAlignment = HorizontalAlignment.Center,

                };
                if (signal.Value is bool)
                {
                    label.FontWeight = FontWeights.Bold;
                    Grid.SetRow(label, rowIndex);
                    Grid.SetColumn(label, 1);
                    grid.Children.Add(label);
                    rowIndex++;
                    if (rowIndex == inputs.Count + 2)
                    {
                        label.BorderThickness = new Thickness(0, 1, 1, 1);
                    }
                }
                if (signal.Value is ushort)
                {
                    label.Foreground = new SolidColorBrush(Color.FromRgb(71, 92, 167));
                    Grid.SetRow(label, rowIndex);
                    Grid.SetColumn(label, 2);
                    grid.Children.Add(label);
                    rowIndex++;
                    if (rowIndex == inputs.Count + 2)
                    {
                        label.BorderThickness = new Thickness(0, 1, 1, 1);
                    }
                }
            }
        }
        private void ConvertedValue(List<ushort> analogSignals, Grid grid)
        {
            int rowIndex = 2;
            foreach (ushort analogSignal in analogSignals)
            {
                Label label = new Label
                {
                    Content = analogSignal,
                    Foreground = new SolidColorBrush(Color.FromRgb(71, 92, 167)),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(192, 192, 192)),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                };
                if (rowIndex == analogSignals.Count + 1)
                {
                    label.BorderThickness = new Thickness(0, 1, 0, 1);
                }
                Grid.SetRow(label, rowIndex);
                Grid.SetColumn(label, 1);
                grid.Children.Add(label);
                rowIndex++;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}