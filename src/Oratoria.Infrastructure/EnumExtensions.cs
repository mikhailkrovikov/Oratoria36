using System.ComponentModel;

namespace Oratoria.Infrastructure
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }
    }
}
