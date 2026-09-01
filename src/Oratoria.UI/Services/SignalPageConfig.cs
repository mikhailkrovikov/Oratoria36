using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Oratoria.Domain.Signals;

namespace Oratoria.UI.Services;

public static class SignalPageConfig
{
    private static Border CreateRowContainer(int rowIndex)
    {
        return new Border
        {
            Background = rowIndex % 2 == 0
                ? new SolidColorBrush(Colors.White)
                : new SolidColorBrush(Color.FromRgb(245, 245, 245)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230)),
            BorderThickness = new Thickness(0, 0, 0, 1),
            Margin = new Thickness(2, 1, 2, 1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(2)
        };
    }

    public static void ConfigureDISignalGrid(Grid grid, ObservableCollection<InputSignal<bool>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33) });
        }

        int rowIndex = 0;
        foreach (var signal in signals)
        {
            var rowContainer = CreateRowContainer(rowIndex);
            Grid.SetRow(rowContainer, rowIndex);
            Grid.SetColumnSpan(rowContainer, 3);
            grid.Children.Add(rowContainer);

            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });
            rowContainer.Child = contentGrid;

            var pinTextBlock = new TextBlock()
            {
                Text = signal.PinNumber.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetColumn(pinTextBlock, 0);
            contentGrid.Children.Add(pinTextBlock);

            var nameTextBlock = new TextBlock()
            {
                Text = signal.Name,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
            };
            Grid.SetColumn(nameTextBlock, 1);
            contentGrid.Children.Add(nameTextBlock);

            var valueCheckBox = new CheckBox()
            {
                Style = (Style)System.Windows.Application.Current.FindResource("ToggleSwitchStyle"),
                VerticalAlignment = VerticalAlignment.Center,
                IsChecked = signal.Value,
                Margin = new Thickness(0, 0, 5, 0)
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
                valueCheckBox.Dispatcher.BeginInvoke(() => valueCheckBox.IsChecked = newValue);
            };

            Grid.SetColumn(valueCheckBox, 2);
            contentGrid.Children.Add(valueCheckBox);

            rowIndex++;
        }
    }

    public static void ConfigureDOSignalGrid(Grid grid, ObservableCollection<OutputSignal<bool>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33) });
        }

        int rowIndex = 0;
        foreach (var signal in signals)
        {
            var rowContainer = CreateRowContainer(rowIndex);
            Grid.SetRow(rowContainer, rowIndex);
            Grid.SetColumnSpan(rowContainer, 3);
            grid.Children.Add(rowContainer);

            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });
            rowContainer.Child = contentGrid;

            var pinTextBlock = new TextBlock()
            {
                Text = signal.PinNumber.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetColumn(pinTextBlock, 0);
            contentGrid.Children.Add(pinTextBlock);

            var nameTextBlock = new TextBlock()
            {
                Text = signal.Name,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
            };
            Grid.SetColumn(nameTextBlock, 1);
            contentGrid.Children.Add(nameTextBlock);

            var valueCheckBox = new CheckBox()
            {
                Style = (Style)System.Windows.Application.Current.FindResource("ToggleSwitchStyle"),
                VerticalAlignment = VerticalAlignment.Center,
                IsChecked = signal.Value,
                Margin = new Thickness(0, 0, 5, 0)
            };

            valueCheckBox.Checked += (sender, e) => signal.Value = true;
            valueCheckBox.Unchecked += (sender, e) => signal.Value = false;

            signal.OnSignalChanged += newValue =>
            {
                valueCheckBox.Dispatcher.BeginInvoke(() => valueCheckBox.IsChecked = newValue);
            };

            Grid.SetColumn(valueCheckBox, 2);
            contentGrid.Children.Add(valueCheckBox);

            rowIndex++;
        }
    }

    public static void ConfigureAISignalGrid(Grid grid, ObservableCollection<InputSignal<double>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33) });
        }

        int rowIndex = 0;
        foreach (var signal in signals)
        {
            var rowContainer = CreateRowContainer(rowIndex);
            Grid.SetRow(rowContainer, rowIndex);
            Grid.SetColumnSpan(rowContainer, 4);
            grid.Children.Add(rowContainer);

            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            rowContainer.Child = contentGrid;

            var pinTextBlock = new TextBlock()
            {
                Text = signal.PinNumber.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetColumn(pinTextBlock, 0);
            contentGrid.Children.Add(pinTextBlock);

            var nameTextBlock = new TextBlock()
            {
                Text = signal.Name,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
            };
            Grid.SetColumn(nameTextBlock, 1);
            contentGrid.Children.Add(nameTextBlock);

            var valueTextBlock = new TextBlock()
            {
                Text = Math.Round(signal.Value, 3).ToString(),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)System.Windows.Application.Current.FindResource("AnalogBlueValueTextBlock"),
                Margin = new Thickness(0, 0, 5, 0)
            };
            Grid.SetColumn(valueTextBlock, 2);
            contentGrid.Children.Add(valueTextBlock);

            var realValueTextBlock = new TextBox()
            {
                Style = (Style)System.Windows.Application.Current.FindResource("TextBoxInput"),
                Height = 25,
                Text = Math.Round(signal.Value, 3).ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };

            realValueTextBlock.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    if (double.TryParse(realValueTextBlock.Text.Replace('.', ','), out double newValue))
                        signal.Value = newValue;
                    else
                        realValueTextBlock.Text = signal.Value.ToString();
                    Keyboard.ClearFocus();
                    FocusManager.SetFocusedElement(grid, null);
                }
            };

            realValueTextBlock.LostFocus += (sender, e) =>
            {
                if (double.TryParse(realValueTextBlock.Text.Replace('.', ','), out double newValue))
                    signal.Value = newValue;
                else
                    realValueTextBlock.Text = signal.Value.ToString();
            };

            Grid.SetColumn(realValueTextBlock, 3);
            contentGrid.Children.Add(realValueTextBlock);

            signal.OnSignalChanged += newValue =>
            {
                valueTextBlock.Dispatcher.BeginInvoke(() => valueTextBlock.Text = Math.Round(newValue, 3).ToString());
                realValueTextBlock.Dispatcher.BeginInvoke(() => realValueTextBlock.Text = Math.Round(newValue, 3).ToString());
            };

            rowIndex++;
        }
    }

    public static void ConfigureAOSignalGrid(Grid grid, ObservableCollection<OutputSignal<double>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33) });
        }

        int rowIndex = 0;
        foreach (var signal in signals)
        {
            var rowContainer = CreateRowContainer(rowIndex);
            Grid.SetRow(rowContainer, rowIndex);
            Grid.SetColumnSpan(rowContainer, 4);
            grid.Children.Add(rowContainer);

            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            rowContainer.Child = contentGrid;

            var pinTextBlock = new TextBlock()
            {
                Text = signal.PinNumber.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetColumn(pinTextBlock, 0);
            contentGrid.Children.Add(pinTextBlock);

            var nameTextBlock = new TextBlock()
            {
                Text = signal.Name,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
            };
            Grid.SetColumn(nameTextBlock, 1);
            contentGrid.Children.Add(nameTextBlock);

            var valueTextBlock = new TextBlock()
            {
                Text = Math.Round(signal.Value, 2).ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Style = (Style)System.Windows.Application.Current.FindResource("AnalogBlueValueTextBlock"),
            };
            Grid.SetColumn(valueTextBlock, 2);
            contentGrid.Children.Add(valueTextBlock);

            var textBox = new TextBox()
            {
                Style = (Style)System.Windows.Application.Current.FindResource("TextBoxInput"),
                Height = 25,
                Text = Math.Round(signal.Value, 2).ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };
            Grid.SetColumn(textBox, 3);
            contentGrid.Children.Add(textBox);

            textBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    if (double.TryParse(textBox.Text.Replace('.', ','), out double newValue))
                        signal.Value = newValue;
                    else
                        textBox.Text = Math.Round(signal.Value, 2).ToString();
                    Keyboard.ClearFocus();
                    FocusManager.SetFocusedElement(grid, null);
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (double.TryParse(textBox.Text.Replace('.', ','), out double newValue))
                    signal.Value = newValue;
                else
                    textBox.Text = Math.Round(signal.Value, 2).ToString();
            };

            signal.OnSignalChanged += newValue =>
            {
                valueTextBlock.Dispatcher.BeginInvoke(() => valueTextBlock.Text = Math.Round(newValue, 2).ToString());
                textBox.Dispatcher.BeginInvoke(() => textBox.Text = Math.Round(newValue, 2).ToString());
            };

            rowIndex++;
        }
    }
}
