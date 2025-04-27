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
            
            for (int i = 0; i < signals.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            int rowIndex = 1;
            foreach (var signal in signals)
            {
                var pinTextBlock = new TextBlock()
                {
                    Text = signal.PinNumber.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(pinTextBlock, rowIndex);
                Grid.SetColumn(pinTextBlock, 0);
                grid.Children.Add(pinTextBlock);

                var nameTextBlock = new TextBlock()
                {
                    Text = signal.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 1);
                grid.Children.Add(nameTextBlock);

                var valueCheckBox = new CheckBox()
                {
                    Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                    IsChecked = signal.Value
                };

                valueCheckBox.Checked += (sender, e) => signal.Value = true;
                valueCheckBox.Unchecked += (sender, e) => signal.Value = false;

                signal.OnSignalChanged += newValue =>
                {
                    valueCheckBox.Dispatcher.Invoke(() => valueCheckBox.IsChecked = newValue);
                };

                Grid.SetRow(valueCheckBox, rowIndex);
                Grid.SetColumn(valueCheckBox, 2);
                grid.Children.Add(valueCheckBox);

                rowIndex++;
            }
        }

        public static void ConfigureDOSignalGrid(Grid grid, ObservableCollection<OutputSignal<bool>> signals)
        {

            for (int i = 0; i < signals.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            int rowIndex = 1;
            foreach (var signal in signals)
            {
                var pinTextBlock = new TextBlock()
                {
                    Text = signal.PinNumber.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(pinTextBlock, rowIndex);
                Grid.SetColumn(pinTextBlock, 0);
                grid.Children.Add(pinTextBlock);

                var nameTextBlock = new TextBlock()
                {
                    Text = signal.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 1);
                grid.Children.Add(nameTextBlock);

                var valueCheckBox = new CheckBox()
                {
                    Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                    IsChecked = signal.Value
                };

                valueCheckBox.Checked += (sender, e) => signal.Value = true;
                valueCheckBox.Unchecked += (sender, e) => signal.Value = false;

                signal.OnSignalChanged += newValue =>
                {
                    valueCheckBox.Dispatcher.Invoke(() => valueCheckBox.IsChecked = newValue);
                };

                Grid.SetRow(valueCheckBox, rowIndex);
                Grid.SetColumn(valueCheckBox, 2);
                grid.Children.Add(valueCheckBox);

                rowIndex++;
            }
        }

        public static void ConfigureAISignalGrid(Grid grid, ObservableCollection<InputSignal<ushort>> signals)
        {
            for (int i = 0; i < signals.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            int rowIndex = 1;
            foreach (var signal in signals)
            {
                var pinTextBlock = new TextBlock()
                {
                    Text = signal.PinNumber.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(pinTextBlock, rowIndex);
                Grid.SetColumn(pinTextBlock, 0);
                grid.Children.Add(pinTextBlock);

                var nameTextBlock = new TextBlock()
                {
                    Text = signal.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 1);
                grid.Children.Add(nameTextBlock);

                var valueTextBlock = new TextBlock()
                {
                    Text = signal.Value.ToString(),
                    FontWeight = FontWeights.Bold,
                    Style = (Style)Application.Current.FindResource("AnalogBlueValueTextBlock"),
                };
                Grid.SetRow(valueTextBlock, rowIndex);
                Grid.SetColumn(valueTextBlock, 2);
                grid.Children.Add(valueTextBlock);

                var realValueTextBlock = new TextBlock()
                {
                    Text = signal.Value.ToString(),
                    Style = (Style)Application.Current.FindResource("AnalogGreyValueTextBlock"),
                };
                Grid.SetRow(realValueTextBlock, rowIndex);
                Grid.SetColumn(realValueTextBlock, 3);
                grid.Children.Add(realValueTextBlock);

                signal.OnSignalChanged += newValue =>
                {
                    valueTextBlock.Dispatcher.Invoke(() => valueTextBlock.Text = newValue.ToString());
                    realValueTextBlock.Dispatcher.Invoke(() => realValueTextBlock.Text = newValue.ToString());
                };

                rowIndex++;
            }
        }

        public static void ConfigureAOSignalGrid(Grid grid, ObservableCollection<OutputSignal<ushort>> signals)
        {

            for (int i = 0; i < signals.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            int rowIndex = 1;
            foreach (var signal in signals)
            {
                var pinTextBlock = new TextBlock()
                {
                    Text = signal.PinNumber.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(pinTextBlock, rowIndex);
                Grid.SetColumn(pinTextBlock, 0);
                grid.Children.Add(pinTextBlock);

                var nameTextBlock = new TextBlock()
                {
                    Text = signal.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                };
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 1);
                grid.Children.Add(nameTextBlock);

                var valueTextBlock = new TextBlock()
                {
                    Text = signal.Value.ToString(),
                    FontWeight = FontWeights.Bold,
                    Style = (Style)Application.Current.FindResource("AnalogBlueValueTextBlock"),
                };
                Grid.SetRow(valueTextBlock, rowIndex);
                Grid.SetColumn(valueTextBlock, 2);
                grid.Children.Add(valueTextBlock);

                var textBox = new TextBox()
                {
                    Style = (Style)Application.Current.FindResource("TextBoxInput"),
                    Height = 20,
                    Text = signal.Value.ToString(),
                    VerticalAlignment = VerticalAlignment.Top,
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
                };

                signal.OnSignalChanged += newValue =>
                {
                    valueTextBlock.Dispatcher.Invoke(() => valueTextBlock.Text = newValue.ToString());
                    textBox.Dispatcher.Invoke(() => textBox.Text = newValue.ToString());
                };

                rowIndex++;
            }
        }
    }
}