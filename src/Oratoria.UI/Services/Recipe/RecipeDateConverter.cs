using System.Globalization;
using System.Windows.Data;

namespace Oratoria.UI.Services.Recipe
{
    public class RecipeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc).ToLocalTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
