using Oratoria36.Service.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI.UserElements
{

    public partial class ValveUE : UserControl
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(State), typeof(ValveUE),
                new PropertyMetadata(State.Off));

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ValveUE));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        public State State
        {
            get => (State)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public ValveUE()
        {
            InitializeComponent();
        }

        private void Valve_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
            e.Handled = true;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Оставлено для совместимости
        }
    }
}