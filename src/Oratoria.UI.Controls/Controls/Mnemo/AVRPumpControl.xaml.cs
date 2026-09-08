using Oratoria.UI.Controls.Controls.Mnemo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class AVRPumpControl : UserControl
    {
        private static readonly Brush OffFill = CreateBrush("#808080");
        private static readonly Brush TransitionFill = CreateBrush("#93C2E4");
        private static readonly Brush OnFill = CreateBrush("#F0F0F0");
        private static readonly Brush WarningIcon = CreateBrush("#F5E11B");
        private static readonly Brush ErrorIconFill = CreateBrush("#E22028");

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(StateColor),
                typeof(AVRPumpControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(AVRPumpControl));

        public static readonly DependencyProperty ErrorIconProperty =
            DependencyProperty.Register(
                nameof(ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(AVRPumpControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(AVRPumpControl));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(AVRPumpControl));

        public AVRPumpControl()
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
            ((AVRPumpControl)d).ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            if (Body is null)
                return;

            Body.Fill = Color switch
            {
                StateColor.Off => OffFill,
                StateColor.On => OnFill,
                _ => TransitionFill
            };

            if (ErrorOverlay is null)
                return;

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

        private static Brush CreateBrush(string hex)
        {
            var brush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(hex)!);
            brush.Freeze();
            return brush;
        }
    }
}
