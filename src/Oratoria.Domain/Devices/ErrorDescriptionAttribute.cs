namespace Oratoria.Domain.Devices
{
    public class ErrorDescriptionAttribute : Attribute
    {
        public readonly string Text;
        public ErrorDescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
