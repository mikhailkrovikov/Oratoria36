using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Oratoria36.Converters
{
    public class BooleanToTextConverter : IValueConverter
    {
        public string TrueText { get; set; } = "ВКЛ";
        public string FalseText { get; set; } = "ВЫКЛ";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? TrueText : FalseText;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}
