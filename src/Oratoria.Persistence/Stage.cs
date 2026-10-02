namespace Oratoria.Persistence
{
    public class Stage
    {
        public int Number { get; set; }
        public double? HeatingTime { get; set; }
        public double? HeatingPower { get; set; }
        public double? HeatingTemp { get; set; }
        public double? Pressure { get; set; }
        public double? Consumption { get; set; }
        public double? SputteringTime { get; set; }
        public double? PreSputteringTime { get; set; }
        public double? Magn1Power { get; set; }
        public double? Magn2Power { get; set; }
        public double? Magn3Power { get; set; }

        public Stage Copy()
        {
            return (Stage)MemberwiseClone();
        }
    }
}
