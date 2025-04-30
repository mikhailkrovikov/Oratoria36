using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria36.UI.UserElements
{
    public partial class PowerButton : UserControl
    {
        public static readonly DependencyProperty PathStrokeThicknessProperty =
            DependencyProperty.Register("PathStrokeThickness", typeof(double), typeof(PowerButton),
                new PropertyMetadata(2.0));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(PowerButton));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(PowerButton));

        public double PathStrokeThickness
        {
            get => (double)GetValue(PathStrokeThicknessProperty);
            set => SetValue(PathStrokeThicknessProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public PowerButton()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) =>
            {
                if (Command?.CanExecute(CommandParameter) == true)
                {
                    Command.Execute(CommandParameter);
                }
            };
        }
    }
}