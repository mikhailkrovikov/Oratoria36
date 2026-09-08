using Oratoria.UI.Controls.Controls.Mnemo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class ManipulatorControl : UserControl
    {
        private static readonly Brush OffFill = CreateBrush("#808080");
        private static readonly Brush TransitionFill = CreateBrush("#93C2E4");
        private static readonly Brush OnFill = CreateBrush("#F0F0F0");
        private static readonly Brush UncertainFill = CreateBrush("#F5E11B");
        private static readonly Brush IndefiniteFill = CreateBrush("#E22028");
        private static readonly Brush WarningIcon = CreateBrush("#F5E11B");
        private static readonly Brush ErrorIconFill = CreateBrush("#E22028");

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(StateColor),
                typeof(ManipulatorControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                nameof(Position),
                typeof(ManipulatorArmPosition),
                typeof(ManipulatorControl),
                new PropertyMetadata(ManipulatorArmPosition.Home, OnAppearanceChanged));

        public static readonly DependencyProperty PositionLabelProperty =
            DependencyProperty.Register(
                nameof(PositionLabel),
                typeof(string),
                typeof(ManipulatorControl));

        public static readonly DependencyProperty ErrorIconProperty =
            DependencyProperty.Register(
                nameof(ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(ManipulatorControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(ManipulatorControl));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(ManipulatorControl));

        public ManipulatorControl()
        {
            InitializeComponent();
            ApplyAppearance();
        }

        public StateColor Color
        {
            get => (StateColor)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ManipulatorArmPosition Position
        {
            get => (ManipulatorArmPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public string? PositionLabel
        {
            get => (string?)GetValue(PositionLabelProperty);
            set => SetValue(PositionLabelProperty, value);
        }

        public ErrorStateIcon ErrorIcon
        {
            get => (ErrorStateIcon)GetValue(ErrorIconProperty);
            set => SetValue(ErrorIconProperty, value);
        }

        public ICommand? Command
        {
            get => (ICommand?)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ManipulatorControl)d).ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            if (Arm is null)
                return;

            var fill = ResolveFill(Color);
            Arm.Fill = fill;
            Joint.Fill = fill;

            ManipulatorRotation.Angle = Position switch
            {
                ManipulatorArmPosition.Module => 180,
                ManipulatorArmPosition.Home => 270,
                ManipulatorArmPosition.Transport => 0,
                _ => ManipulatorRotation.Angle
            };

            if (ErrorIcon == ErrorStateIcon.None)
            {
                ErrorOverlay.Visibility = Visibility.Collapsed;
                return;
            }

            ErrorOverlay.Visibility = Visibility.Visible;
            ErrorBadge.Fill = ErrorIcon switch
            {
                ErrorStateIcon.Warning => WarningIcon,
                ErrorStateIcon.Error => ErrorIconFill,
                _ => Brushes.Transparent
            };
        }

        private static Brush ResolveFill(StateColor color) => color switch
        {
            StateColor.Off => OffFill,
            StateColor.On => OnFill,
            StateColor.Uncertain => UncertainFill,
            StateColor.Indefinite => IndefiniteFill,
            _ => TransitionFill
        };

        private static Brush CreateBrush(string hex)
        {
            var brush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(hex)!);
            brush.Freeze();
            return brush;
        }
    }
}
