using Oratoria36.Service.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Oratoria36.UI.UserElements
{
    public partial class MagnetronsUE : UserControl
    {
        public static readonly DependencyProperty Magnetron1ValueProperty =
            DependencyProperty.Register("Magnetron1Value", typeof(int), typeof(MagnetronsUE),
                new PropertyMetadata(100));

        public static readonly DependencyProperty Magnetron2ValueProperty =
            DependencyProperty.Register("Magnetron2Value", typeof(int), typeof(MagnetronsUE),
                new PropertyMetadata(100));

        public static readonly DependencyProperty Magnetron3ValueProperty =
            DependencyProperty.Register("Magnetron3Value", typeof(int), typeof(MagnetronsUE),
                new PropertyMetadata(100));

        public static readonly DependencyProperty M1StateProperty =
            DependencyProperty.Register("Magnetron1State", typeof(State), typeof(MagnetronsUE),
                new PropertyMetadata(State.Transition));

        public static readonly DependencyProperty M2StateProperty =
            DependencyProperty.Register("Magnetron2State", typeof(State), typeof(MagnetronsUE),
                new PropertyMetadata(State.Transition));

        public static readonly DependencyProperty M3StateProperty =
            DependencyProperty.Register("Magnetron3State", typeof(State), typeof(MagnetronsUE),
                new PropertyMetadata(State.Transition));

        public int Magnetron1Value
        {
            get { return (int)GetValue(Magnetron1ValueProperty); }
            set { SetValue(Magnetron1ValueProperty, value); }
        }

        public int Magnetron2Value
        {
            get { return (int)GetValue(Magnetron2ValueProperty); }
            set { SetValue(Magnetron2ValueProperty, value); }
        }

        public int Magnetron3Value
        {
            get { return (int)GetValue(Magnetron3ValueProperty); }
            set { SetValue(Magnetron3ValueProperty, value); }
        }

        public State Magnetron1State
        {
            get { return (State)GetValue(M1StateProperty); }
            set { SetValue(M1StateProperty, value); }
        }

        public State Magnetron2State
        {
            get { return (State)GetValue(M2StateProperty); }
            set { SetValue(M2StateProperty, value); }
        }

        public State Magnetron3State
        {
            get { return (State)GetValue(M3StateProperty); }
            set { SetValue(M3StateProperty, value); }
        }

        public MagnetronsUE()
        {
            InitializeComponent();
        }
    }
}