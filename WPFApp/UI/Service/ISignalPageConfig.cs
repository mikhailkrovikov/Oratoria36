using Oratoria36.Models.Connection;
using Oratoria36.Models.Signals;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

public interface ISignalPageConfig
{
    public NetConfig NetConfig { get; }

    private static Border CreateRowContainer(int rowIndex)
    {
        var rowContainer = new Border
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

        return rowContainer;
    }

    public void ConfigureDISignalGrid(Grid grid, ObservableCollection<InputSignal<bool>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(32) });
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
                Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                VerticalAlignment = VerticalAlignment.Center,
                IsChecked = signal.Value,
                IsEnabled = !NetConfig.IsConnected,
                Margin = new Thickness(0, 0, 5, 0)
            };

            valueCheckBox.Checked += (sender, e) => signal.Value = true;
            valueCheckBox.Unchecked += (sender, e) => signal.Value = false;

            signal.OnSignalChanged += newValue =>
            {
                valueCheckBox.Dispatcher.Invoke(() => valueCheckBox.IsChecked = newValue);
            };

            Grid.SetColumn(valueCheckBox, 2);
            contentGrid.Children.Add(valueCheckBox);

            rowIndex++;
        }
    }

    public void ConfigureDOSignalGrid(Grid grid, ObservableCollection<OutputSignal<bool>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(32) });
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
                Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                VerticalAlignment = VerticalAlignment.Center,
                IsChecked = signal.Value,
                Margin = new Thickness(0, 0, 5, 0)
            };

            valueCheckBox.Checked += (sender, e) => signal.Value = true;
            valueCheckBox.Unchecked += (sender, e) => signal.Value = false;

            signal.OnSignalChanged += newValue =>
            {
                valueCheckBox.Dispatcher.Invoke(() => valueCheckBox.IsChecked = newValue);
            };

            Grid.SetColumn(valueCheckBox, 2);
            contentGrid.Children.Add(valueCheckBox);

            rowIndex++;
        }
    }

    public void ConfigureAISignalGrid(Grid grid, ObservableCollection<InputSignal<ushort>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(32) });
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
                Text = signal.Value.ToString(),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)Application.Current.FindResource("AnalogBlueValueTextBlock"),
                Margin = new Thickness(0, 0, 5, 0)
            };
            Grid.SetColumn(valueTextBlock, 2);
            contentGrid.Children.Add(valueTextBlock);

            var realValueTextBlock = new TextBlock()
            {
                Text = signal.Value.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)Application.Current.FindResource("AnalogGreyValueTextBlock"),
                Margin = new Thickness(0, 0, 5, 0)
            };
            Grid.SetColumn(realValueTextBlock, 3);
            contentGrid.Children.Add(realValueTextBlock);

            signal.OnSignalChanged += newValue =>
            {
                valueTextBlock.Dispatcher.Invoke(() => valueTextBlock.Text = newValue.ToString());
                realValueTextBlock.Dispatcher.Invoke(() => realValueTextBlock.Text = newValue.ToString());
            };

            rowIndex++;
        }
    }

    public void ConfigureAOSignalGrid(Grid grid, ObservableCollection<OutputSignal<ushort>> signals)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(32) });
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
                Text = signal.Value.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Style = (Style)Application.Current.FindResource("AnalogBlueValueTextBlock"),
            };
            Grid.SetColumn(valueTextBlock, 2);
            contentGrid.Children.Add(valueTextBlock);

            var textBox = new TextBox()
            {
                Style = (Style)Application.Current.FindResource("TextBoxInput"),
                Height = 20,
                Text = signal.Value.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };
            Grid.SetColumn(textBox, 3);
            contentGrid.Children.Add(textBox);

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
