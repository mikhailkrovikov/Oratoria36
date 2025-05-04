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
    /// Логика взаимодействия для HeaterUE.xaml
    /// </summary>
    public partial class HeaterUE : UserControl
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(State), typeof(HeaterUE),
                new PropertyMetadata(State.Transition));

        public State State
        {
            get => (State)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public HeaterUE()
        {
            InitializeComponent();
        }
    }
}
