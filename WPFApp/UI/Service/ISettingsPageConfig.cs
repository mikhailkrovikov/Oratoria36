using Oratoria36.Models.Settings;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria36.UI.Service
{
    public interface ISettingsPageConfig
    {
        public void ConfigCommonSettings(Grid grid, ObservableCollection<object> settings)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            for (int i = 0; i < settings.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            int rowIndex = 0;
            foreach (var settingObj in settings)
            {
                dynamic setting = settingObj;

                var nameTextBlock = new TextBlock()
                {
                    Text = setting.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 0);
                grid.Children.Add(nameTextBlock);

                var deviceTextBlock = new TextBlock()
                {
                    Text = setting.Device,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    VerticalAlignment = VerticalAlignment.Center,    
                    Margin = new Thickness(5)
                };
                Grid.SetRow(deviceTextBlock, rowIndex);
                Grid.SetColumn(deviceTextBlock, 1);
                grid.Children.Add(deviceTextBlock);

                var unitTextBlock = new TextBlock()
                {
                    Text = setting.Unit,
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    VerticalAlignment = VerticalAlignment.Center,    
                    Margin = new Thickness(5)
                };
                Grid.SetRow(unitTextBlock, rowIndex);
                Grid.SetColumn(unitTextBlock, 2);
                grid.Children.Add(unitTextBlock);

                var maxTextBlock = new TextBlock()
                {
                    Text = "-",
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    VerticalAlignment = VerticalAlignment.Center,                  
                    Margin = new Thickness(5)
                };
                Grid.SetRow(maxTextBlock, rowIndex);
                Grid.SetColumn(maxTextBlock, 3);
                grid.Children.Add(maxTextBlock);

                
                var minTextBlock = new TextBlock()
                {
                    Text = "-",
                    Foreground = new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    VerticalAlignment = VerticalAlignment.Center,
                    
                    Margin = new Thickness(5)
                };
                Grid.SetRow(minTextBlock, rowIndex);
                Grid.SetColumn(minTextBlock, 4);
                grid.Children.Add(minTextBlock);


                Type settingType = setting.GetType();
                Type[] genericArgs = settingType.GetGenericArguments();
                Type valueType = genericArgs.Length > 0 ? genericArgs[0] : typeof(object);

                if (valueType == typeof(bool))
                {
                    var checkBox = new CheckBox()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Style = (Style)Application.Current.FindResource("ToggleSwitchStyle"),
                        Margin = new Thickness(5)
                    };

                    try
                    {
                        bool value = setting.Value;
                        checkBox.IsChecked = value;
                    }
                    catch
                    {
                        checkBox.IsChecked = false;
                    }

                    checkBox.Checked += (sender, e) => {
                        setting.Value = true;
                    };

                    checkBox.Unchecked += (sender, e) => {
                        setting.Value = false;
                    };

                    Grid.SetRow(checkBox, rowIndex);
                    Grid.SetColumn(checkBox, 5);
                    grid.Children.Add(checkBox);
                }
                else
                {
                    var textBox = new TextBox()
                    {
                        Style = (Style)Application.Current.FindResource("TextBoxInput"),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5),
                        Width = 80
                    };


                    try
                    {
                        var value = setting.Value;
                        textBox.Text = value.ToString();
                    }
                    catch
                    {
                        textBox.Text = "0";
                    }

                    textBox.LostFocus += (sender, e) => 
                    {
                        UpdateNumericValue(textBox, setting);
                    };

                    textBox.KeyDown += (sender, e) => 
                    {
                        if (e.Key == Key.Enter)
                        {
                            UpdateNumericValue(textBox, setting);
                            Keyboard.ClearFocus();
                            FocusManager.SetFocusedElement(grid, null);
                        }
                    };

                    Grid.SetRow(textBox, rowIndex);
                    Grid.SetColumn(textBox, 5);
                    grid.Children.Add(textBox);
                }

                rowIndex++;
            }
        }

        private static void UpdateNumericValue(TextBox textBox, dynamic setting)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return;

            try
            {
                Type settingType = setting.GetType();
                Type[] genericArgs = settingType.GetGenericArguments();
                Type valueType = genericArgs[0];

                if (valueType == typeof(double))
                {
                    if (double.TryParse(textBox.Text, out double doubleValue))
                    {
                        setting.Value = doubleValue;
                    }
                }
                else if (valueType == typeof(int))
                {
                    if (int.TryParse(textBox.Text, out int intValue))
                    {
                        setting.Value = intValue;
                    }
                }
                else if (valueType == typeof(ushort))
                {
                    if (ushort.TryParse(textBox.Text, out ushort ushortValue))
                    {
                        setting.Value = ushortValue;
                    }
                }
                else
                {
                    if (double.TryParse(textBox.Text, out double doubleValue))
                    {
                        setting.Value = Convert.ChangeType(doubleValue, valueType);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var value = setting.Value;
                    textBox.Text = value.ToString();
                }
                catch
                {
                    textBox.Text = "0";
                }
            }
        }
    }
}
