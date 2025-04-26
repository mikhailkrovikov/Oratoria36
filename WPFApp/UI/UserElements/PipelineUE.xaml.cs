using Oratoria36.Service.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Oratoria36.UI.UserElements
{
    public partial class PipelineUE : UserControl
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(State), typeof(PipelineUE),
                new PropertyMetadata(State.Transition));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(double), typeof(PipelineUE),
                new PropertyMetadata(0.0));

        public State State
        {
            get => (State)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public double Direction
        {
            get => (double)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public PipelineUE()
        {
            InitializeComponent();
            Height = 20;
            MinHeight = 20;
            MaxHeight = 20;
        }
    }
}