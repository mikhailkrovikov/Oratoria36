using Oratoria.UI.Controls.Controls.Mnemo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class MagnetronsControl : UserControl
    {
        private static readonly Brush OffFill = CreateBrush("#808080");
        private static readonly Brush TransitionFill = CreateBrush("#93C2E4");
        private static readonly Brush OnFill = CreateBrush("#F0F0F0");
        private static readonly Brush WarningIcon = CreateBrush("#F5E11B");
        private static readonly Brush ErrorIconFill = CreateBrush("#E22028");

        public static readonly DependencyProperty M1ColorProperty =
            DependencyProperty.Register(
                nameof(M1Color),
                typeof(StateColor),
                typeof(MagnetronsControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty M1ErrorIconProperty =
            DependencyProperty.Register(
                nameof(M1ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(MagnetronsControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty M1CommandProperty =
            DependencyProperty.Register(
                nameof(M1Command),
                typeof(ICommand),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M1CommandParameterProperty =
            DependencyProperty.Register(
                nameof(M1CommandParameter),
                typeof(object),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M1ValueProperty =
            DependencyProperty.Register(
                nameof(M1Value),
                typeof(int),
                typeof(MagnetronsControl),
                new PropertyMetadata(100));

        public static readonly DependencyProperty M2ColorProperty =
            DependencyProperty.Register(
                nameof(M2Color),
                typeof(StateColor),
                typeof(MagnetronsControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty M2ErrorIconProperty =
            DependencyProperty.Register(
                nameof(M2ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(MagnetronsControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty M2CommandProperty =
            DependencyProperty.Register(
                nameof(M2Command),
                typeof(ICommand),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M2CommandParameterProperty =
            DependencyProperty.Register(
                nameof(M2CommandParameter),
                typeof(object),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M2ValueProperty =
            DependencyProperty.Register(
                nameof(M2Value),
                typeof(int),
                typeof(MagnetronsControl),
                new PropertyMetadata(100));

        public static readonly DependencyProperty M3ColorProperty =
            DependencyProperty.Register(
                nameof(M3Color),
                typeof(StateColor),
                typeof(MagnetronsControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty M3ErrorIconProperty =
            DependencyProperty.Register(
                nameof(M3ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(MagnetronsControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty M3CommandProperty =
            DependencyProperty.Register(
                nameof(M3Command),
                typeof(ICommand),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M3CommandParameterProperty =
            DependencyProperty.Register(
                nameof(M3CommandParameter),
                typeof(object),
                typeof(MagnetronsControl));

        public static readonly DependencyProperty M3ValueProperty =
            DependencyProperty.Register(
                nameof(M3Value),
                typeof(int),
                typeof(MagnetronsControl),
                new PropertyMetadata(100));

        public MagnetronsControl()
        {
            InitializeComponent();
            ApplyAppearance();
        }

        public StateColor M1Color
        {
            get => (StateColor)GetValue(M1ColorProperty);
            set => SetValue(M1ColorProperty, value);
        }

        public ErrorStateIcon M1ErrorIcon
        {
            get => (ErrorStateIcon)GetValue(M1ErrorIconProperty);
            set => SetValue(M1ErrorIconProperty, value);
        }

        public ICommand? M1Command
        {
            get => (ICommand?)GetValue(M1CommandProperty);
            set => SetValue(M1CommandProperty, value);
        }

        public object? M1CommandParameter
        {
            get => GetValue(M1CommandParameterProperty);
            set => SetValue(M1CommandParameterProperty, value);
        }

        public int M1Value
        {
            get => (int)GetValue(M1ValueProperty);
            set => SetValue(M1ValueProperty, value);
        }

        public StateColor M2Color
        {
            get => (StateColor)GetValue(M2ColorProperty);
            set => SetValue(M2ColorProperty, value);
        }

        public ErrorStateIcon M2ErrorIcon
        {
            get => (ErrorStateIcon)GetValue(M2ErrorIconProperty);
            set => SetValue(M2ErrorIconProperty, value);
        }

        public ICommand? M2Command
        {
            get => (ICommand?)GetValue(M2CommandProperty);
            set => SetValue(M2CommandProperty, value);
        }

        public object? M2CommandParameter
        {
            get => GetValue(M2CommandParameterProperty);
            set => SetValue(M2CommandParameterProperty, value);
        }

        public int M2Value
        {
            get => (int)GetValue(M2ValueProperty);
            set => SetValue(M2ValueProperty, value);
        }

        public StateColor M3Color
        {
            get => (StateColor)GetValue(M3ColorProperty);
            set => SetValue(M3ColorProperty, value);
        }

        public ErrorStateIcon M3ErrorIcon
        {
            get => (ErrorStateIcon)GetValue(M3ErrorIconProperty);
            set => SetValue(M3ErrorIconProperty, value);
        }

        public ICommand? M3Command
        {
            get => (ICommand?)GetValue(M3CommandProperty);
            set => SetValue(M3CommandProperty, value);
        }

        public object? M3CommandParameter
        {
            get => GetValue(M3CommandParameterProperty);
            set => SetValue(M3CommandParameterProperty, value);
        }

        public int M3Value
        {
            get => (int)GetValue(M3ValueProperty);
            set => SetValue(M3ValueProperty, value);
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MagnetronsControl)d).ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            ApplyMagnetronAppearance(M1Color, M1ErrorIcon, M1Body, M1ErrorOverlay, M1ErrorBadge);
            ApplyMagnetronAppearance(M2Color, M2ErrorIcon, M2Body, M2ErrorOverlay, M2ErrorBadge);
            ApplyMagnetronAppearance(M3Color, M3ErrorIcon, M3Body, M3ErrorOverlay, M3ErrorBadge);
        }

        private static void ApplyMagnetronAppearance(
            StateColor color,
            ErrorStateIcon errorIcon,
            Shape? body,
            FrameworkElement? overlay,
            Shape? badge)
        {
            if (body is null)
                return;

            body.Fill = color switch
            {
                StateColor.Off => OffFill,
                StateColor.On => OnFill,
                _ => TransitionFill
            };

            if (overlay is null || badge is null)
                return;

            if (errorIcon == ErrorStateIcon.None)
            {
                overlay.Visibility = Visibility.Collapsed;
                return;
            }

            overlay.Visibility = Visibility.Visible;
            badge.Fill = errorIcon switch
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
