using System.Windows;
using System.Windows.Controls;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class PressureControl : UserControl
    {
        public static readonly DependencyProperty ShowBottomLabelsProperty =
            DependencyProperty.Register(
                nameof(ShowBottomLabels),
                typeof(Visibility),
                typeof(PressureControl),
                new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty HighVacuumProperty =
            DependencyProperty.Register(
                nameof(HighVacuum),
                typeof(string),
                typeof(PressureControl),
                new PropertyMetadata("0"));

        public static readonly DependencyProperty LowVacuumProperty =
            DependencyProperty.Register(
                nameof(LowVacuum),
                typeof(string),
                typeof(PressureControl),
                new PropertyMetadata("0"));

        public PressureControl()
        {
            InitializeComponent();
        }

        public Visibility ShowBottomLabels
        {
            get => (Visibility)GetValue(ShowBottomLabelsProperty);
            set => SetValue(ShowBottomLabelsProperty, value);
        }

        public string? HighVacuum
        {
            get => (string?)GetValue(HighVacuumProperty);
            set => SetValue(HighVacuumProperty, value);
        }

        public string? LowVacuum
        {
            get => (string?)GetValue(LowVacuumProperty);
            set => SetValue(LowVacuumProperty, value);
        }
    }
}
