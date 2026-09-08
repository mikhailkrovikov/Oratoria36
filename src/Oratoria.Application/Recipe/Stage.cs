using Oratoria.Domain.Algorithms;

namespace Oratoria.Application.Recipe
{
    public class Stage
    {
        public int Number { get; set; }
        public double HeatingTime
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                field = value;
            }
        }

        public double SputteringTime
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                field = value;
            }
        }

        public double PreSputteringTime
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                field = value;
            }
        }

        public double HeatingPower
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 4000)
                    value = 4000;
                field = value;
            }
        }

        public double HeatingTemp
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 400)
                    value = 400;
                field = value;
            }
        }

        public double Magn1Power
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 4000)
                    value = 4000;
                field = value;
            }
        }

        public double Magn2Power
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 4000)
                    value = 4000;
                field = value;
            }
        }

        public double Magn3Power
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 4000)
                    value = 4000;
                field = value;
            }
        }

        public double Consumption
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 9)
                    value = 9;
                field = value;
            }
        }

        public double Pressure
        {
            get => field;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 9)
                    value = 9;
                field = value;
            }
        }

    }
}
