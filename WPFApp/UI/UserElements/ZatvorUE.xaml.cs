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
    /// Логика взаимодействия для Zatvor.xaml
    /// </summary>
    public partial class ZatvorUE : UserControl
    {
        public static readonly DependencyProperty StateProperty =
    DependencyProperty.Register("State", typeof(State), typeof(ZatvorUE),
        new PropertyMetadata(State.Transition));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ZatvorUE));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ZatvorUE));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ZatvorUE));

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

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public ZatvorUE()
        {
            InitializeComponent();
        }
    }
}
