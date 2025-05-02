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
    /// Логика взаимодействия для Manipulator.xaml
    /// </summary>
    public partial class ManipulatorUE : UserControl
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(ManipulatorPosition), typeof(ManipulatorUE),
                new PropertyMetadata(ManipulatorPosition.Transport, OnPositionChanged));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(ManipulatorPosition), typeof(ManipulatorUE),
                new PropertyMetadata(ManipulatorPosition.Home));

        public ManipulatorPosition Position
        {
            get { return (ManipulatorPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public State State
        {
            get { return (State)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        public ManipulatorUE()
        {
            InitializeComponent();
            UpdatePosition();
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ManipulatorUE manipulator = d as ManipulatorUE;
            manipulator.UpdatePosition();
        }

        private void UpdatePosition()
        {
            switch (Position)
            {
                case ManipulatorPosition.Module:
                    ManipulatorRotation.Angle = 180;
                    break;
                case ManipulatorPosition.Home:
                    ManipulatorRotation.Angle = 270;
                    break;
                case ManipulatorPosition.Transport:
                    ManipulatorRotation.Angle = 0;
                    break;
            }
        }
    }
}
