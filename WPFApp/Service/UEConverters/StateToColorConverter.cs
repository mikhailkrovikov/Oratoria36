using Oratoria36.Service.Enums;
using System.Windows.Data;
using System.Windows.Media;

namespace Oratoria36.Service
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is State state)
            {
                return state switch
                {
                    State.Off => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#808080")),
                    State.Transition => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93C2E4")),
                    State.On => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0")),
                    State.Warning => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5E11B")),
                    State.Error => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E22028")),
                    _ => Brushes.Transparent
                };
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}