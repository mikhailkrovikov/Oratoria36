using Oratoria36.Models.Signals;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria36.UI.Service
{
    public interface ISignalPageConfig
    {
        public static void ConfigureDISignalGrid(Grid grid, ObservableCollection<InputSignal<bool>> signals)
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

                valueCheckBox.Checked += (sender, e) =>
                {
                    signal.Value = true;
                };
                valueCheckBox.Unchecked += (sender, e) =>
                {
                    signal.Value = false;
                };
                signal.OnSignalChanged += newValue =>
                {
                    valueCheckBox.IsChecked = newValue;
                };

                Grid.SetRow(valueCheckBox, rowIndex);
                Grid.SetColumn(valueCheckBox, 2);
                grid.Children.Add(valueCheckBox);

                rowIndex++;
            }
        }
        public static void ConfigureDOSignalGrid(Grid grid, ObservableCollection<OutputSignal<bool>> signals)
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
                valueCheckBox.Checked += (sender, e) =>
                {
                    signal.Value = true;
                };
                valueCheckBox.Unchecked += (sender, e) =>
                {
                    signal.Value = false;
                };
                signal.OnSignalChanged += newValue =>
                {
                    valueCheckBox.IsChecked = newValue;
                };
                Grid.SetRow(valueCheckBox, rowIndex);
                Grid.SetColumn(valueCheckBox, 2);
                grid.Children.Add(valueCheckBox);

                rowIndex++;
            }
        }
        public static void ConfigureAISignalGrid(Grid grid, ObservableCollection<InputSignal<ushort>> signals)
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
        public static void ConfigureAOSignalGrid(Grid grid, ObservableCollection<OutputSignal<ushort>> signals)
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

                var textBox = new TextBox()
                {
                    Style = (Style)Application.Current.FindResource("TextBoxInput"),
                    Height = 20,
                    Text = signal.Value.ToString(),
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(textBox, rowIndex);
                Grid.SetColumn(textBox, 3);
                grid.Children.Add(textBox);

                textBox.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        if (ushort.TryParse(textBox.Text, out ushort newValue))
                            signal.Value = newValue;
                        else
                            textBox.Text = signal.Value.ToString();
                        Keyboard.ClearFocus();
                        FocusManager.SetFocusedElement(grid, null);
                    }
                };
                textBox.LostFocus += (sender, e) =>
                {
                    if (ushort.TryParse(textBox.Text, out ushort newValue))
                        signal.Value = newValue;
                    else
                        textBox.Text = signal.Value.ToString();
                    Keyboard.ClearFocus();
                    FocusManager.SetFocusedElement(grid, null);
                };
                signal.OnSignalChanged += newValue =>
                {
                    textBox.Text = newValue.ToString();
                };

                rowIndex++;
            }
        }
    }
}