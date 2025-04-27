using Oratoria36.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oratoria36.UI.UserElements
{
    /// <summary>
    /// Логика взаимодействия для RRGUe.xaml
    /// </summary>
    public partial class RRGUe : UserControl
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(State), typeof(RRGUe),
                new PropertyMetadata(State.Transition));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(RRGUe));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(RRGUe));

        public static readonly DependencyProperty RRGSetPointProperty = 
            DependencyProperty.Register("RRGSetPoint", typeof(double), typeof(RRGUe),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty RRGRealValueProperty =
            DependencyProperty.Register("RRGRealValue", typeof(double), typeof(RRGUe),
                new PropertyMetadata(0.0));

        public State State
        {
            get => (State)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
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
        public double RRGSetPoint
        {
            get => (double)GetValue(RRGSetPointProperty);
            set => SetValue(RRGSetPointProperty, value);
        }
        public double RRGRealValue
        {
            get => (double)GetValue(RRGRealValueProperty);
            set => SetValue(RRGRealValueProperty, value);
        }
        public RRGUe()
        {
            InitializeComponent();
        }
    }
}
