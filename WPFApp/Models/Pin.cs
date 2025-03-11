namespace Oratoria36.Models
{
    public class Pin
    {
        public int NumberOfPin { get; set; }
        public string Name { get; set; }
        public Pin(int numberOfPin, string name)
        {
            Name = name;
            NumberOfPin = numberOfPin;
        }
    }
}
