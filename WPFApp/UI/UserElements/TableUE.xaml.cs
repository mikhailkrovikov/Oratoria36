using Oratoria36.Service.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Oratoria36.UI.UserElements
{
    public partial class TableUE : UserControl
    {
        public static readonly DependencyProperty RollbackStateProperty =
            DependencyProperty.Register("RollbackState", typeof(State), typeof(TableUE),
                new PropertyMetadata(State.Off));

        public static readonly DependencyProperty NeutralStateProperty =
            DependencyProperty.Register("NeutralState", typeof(State), typeof(TableUE),
                new PropertyMetadata(State.Off));

        public static readonly DependencyProperty ProcessingStateProperty =
            DependencyProperty.Register("ProcessingState", typeof(State), typeof(TableUE),
                new PropertyMetadata(State.Off));

        public State RollbackState
        {
            get { return (State)GetValue(RollbackStateProperty); }
            set { SetValue(RollbackStateProperty, value); }
        }

        public State NeutralState
        {
            get { return (State)GetValue(NeutralStateProperty); }
            set { SetValue(NeutralStateProperty, value); }
        }

        public State ProcessingState
        {
            get { return (State)GetValue(ProcessingStateProperty); }
            set { SetValue(ProcessingStateProperty, value); }
        }

        public TableUE()
        {
            InitializeComponent();
        }
    }
}
