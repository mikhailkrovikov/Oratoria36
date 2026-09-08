using System.Windows;
using System.Windows.Controls;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class GasLabelControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(GasLabelControl));

        public GasLabelControl()
        {
            InitializeComponent();
        }

        public string? Label
        {
            get => (string?)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
    }
}
