using Oratoria.UI.Controls.Controls.Mnemo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class ThrottleControl : UserControl
    {
        private static readonly Brush OffFill = CreateBrush("#808080");
        private static readonly Brush TransitionFill = CreateBrush("#93C2E4");
        private static readonly Brush OnFill = CreateBrush("#F0F0F0");
        private static readonly Brush UncertainFill = CreateBrush("#F5E11B");
        private static readonly Brush IndefiniteFill = CreateBrush("#E22028");
        private static readonly Brush WarningIcon = CreateBrush("#F5E11B");
        private static readonly Brush ErrorIconFill = CreateBrush("#E22028");

        private static readonly Geometry CloseFlap = CreateGeometry("M50,50 L50,90");
        private static readonly Geometry OpenFlap = CreateGeometry("M10,70 L90,70");
        private static readonly Geometry ThrottlingFlap = CreateGeometry("M10,50 L90,90");

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(StateColor),
                typeof(ThrottleControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(ThrottleControl));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                nameof(Position),
                typeof(ThrottleFlapPosition),
                typeof(ThrottleControl),
                new PropertyMetadata(ThrottleFlapPosition.Indefinite, OnAppearanceChanged));

        public static readonly DependencyProperty PositionLabelProperty =
            DependencyProperty.Register(
                nameof(PositionLabel),
                typeof(string),
                typeof(ThrottleControl));

        public static readonly DependencyProperty ErrorIconProperty =
            DependencyProperty.Register(
                nameof(ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(ThrottleControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(ThrottleControl));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(ThrottleControl));

        public ThrottleControl()
        {
            InitializeComponent();
            ApplyAppearance();
        }

        public StateColor Color
        {
            get => (StateColor)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public string? Label
        {
            get => (string?)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public ThrottleFlapPosition Position
        {
            get => (ThrottleFlapPosition)GetValue(PositionProperty);
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
            ((ThrottleControl)d).ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            if (Body is null)
                return;

            Body.Fill = ResolveFill(Color);
            Flap.Data = Position switch
            {
                ThrottleFlapPosition.Close => CloseFlap,
                ThrottleFlapPosition.Open => OpenFlap,
                _ => ThrottlingFlap
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

        private static Geometry CreateGeometry(string data)
        {
            var geometry = Geometry.Parse(data);
            geometry.Freeze();
            return geometry;
        }

        private static Brush CreateBrush(string hex)
        {
            var brush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(hex)!);
            brush.Freeze();
            return brush;
        }
    }
}
