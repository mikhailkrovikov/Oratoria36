using Oratoria.UI.Controls.Controls.Mnemo;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Oratoria.UI.Converters
{
    public class PipeColorToBrushConverter : IValueConverter
    {
        private static readonly Brush OnStroke = CreateBrush("#93C2E4");
        private static readonly Brush OffStroke = CreateBrush("#A0A0A4");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is PipeColor.On ? OnStroke : OffStroke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Brush CreateBrush(string hex)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex)!);
            brush.Freeze();
            return brush;
        }
    }
}
