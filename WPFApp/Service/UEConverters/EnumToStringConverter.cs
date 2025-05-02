using Oratoria36.Service.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Oratoria36.Service
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value is ManipulatorPosition)
            {
                switch ((ManipulatorPosition)value)
                {
                    case ManipulatorPosition.Transport:
                        return "Транспорт (поз.1)";
                    case ManipulatorPosition.Home:
                        return "Исходная (поз.2)";
                    case ManipulatorPosition.Module:
                        return "Модуль (поз.3)";
                    default:
                        return value.ToString();
                }
            }
            else if (value is State)
            {
                switch ((State)value)
                {
                    case State.Off:
                        return "Закрыт";
                    case State.Transition:
                        return "Переходное";
                    case State.On:
                        return "Открыт";
                    case State.Warning:
                        return "Предупреждение";
                    case State.Error:
                        return "Авария";
                    default:
                        return value.ToString();
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
