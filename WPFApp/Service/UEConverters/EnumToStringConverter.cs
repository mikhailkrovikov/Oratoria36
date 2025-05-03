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
            else if(value is ManipulatorErrors)
            {
                switch((ManipulatorErrors)value)
                    {
                    case ManipulatorErrors.None:
                        return "";
                    case ManipulatorErrors.Error1_1:
                        return "Манипулятор не в исходном положении";
                    case ManipulatorErrors.Error1_4:
                        return "Манипулятор не опустился к ложементу";
                    case ManipulatorErrors.Error1_5:
                        return "Манипулятор не поднялся от ложемента к исходному";
                    case ManipulatorErrors.Error1_6:
                        return "Манипулятор не опустился к каретке";
                    case ManipulatorErrors.Error1_7:
                        return "Манипулятор не поднялся от каретки к исходному";
                    case ManipulatorErrors.Error1_8:
                        return "Наличие пластины в манипуляторе";
                    case ManipulatorErrors.Error1_9:
                        return "Манипулятор не поставил пластину в каретку";
                    case ManipulatorErrors.Error1_10:
                        return "Манипулятор не взял пластину из каретки";
                    case ManipulatorErrors.Error1_11:
                        return "Манипулятор не постаавил пластину в ложемент";
                    case ManipulatorErrors.Error1_12:
                        return "Манипулятор не взял пластину из ложемента";
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
