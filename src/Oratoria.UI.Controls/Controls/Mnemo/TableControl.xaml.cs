using Oratoria.UI.Controls.Controls.Mnemo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Oratoria.UI.Controls.Mnemo
{
    public partial class TableControl : UserControl
    {
        private static readonly Brush OffFill = CreateBrush("#808080");
        private static readonly Brush TransitionFill = CreateBrush("#93C2E4");
        private static readonly Brush OnFill = CreateBrush("#F0F0F0");
        private static readonly Brush UncertainFill = CreateBrush("#F5E11B");
        private static readonly Brush IndefiniteFill = CreateBrush("#E22028");
        private static readonly Brush WarningIcon = CreateBrush("#F5E11B");
        private static readonly Brush ErrorIconFill = CreateBrush("#E22028");
        private static readonly Brush ActiveText = CreateBrush("#3F3F3F");
        private static readonly Brush InactiveText = CreateBrush("#919191");

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(StateColor),
                typeof(TableControl),
                new PropertyMetadata(StateColor.Transition, OnAppearanceChanged));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                nameof(Position),
                typeof(TableSlotPosition),
                typeof(TableControl),
                new PropertyMetadata(TableSlotPosition.Indefinite, OnAppearanceChanged));

        public static readonly DependencyProperty ErrorIconProperty =
            DependencyProperty.Register(
                nameof(ErrorIcon),
                typeof(ErrorStateIcon),
                typeof(TableControl),
                new PropertyMetadata(ErrorStateIcon.None, OnAppearanceChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(TableControl));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(TableControl));

        public TableControl()
        {
            InitializeComponent();
            ApplyAppearance();
        }

        public StateColor Color
        {
            get => (StateColor)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public TableSlotPosition Position
        {
            get => (TableSlotPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
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
            ((TableControl)d).ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            if (ProcessingSlot is null)
                return;

            var fill = ResolveFill(Color);
            ApplySlot(ProcessingText, ProcessingSlot, Position == TableSlotPosition.Processing, fill);
            ApplySlot(HomeText, HomeSlot, Position == TableSlotPosition.Home, fill);
            ApplySlot(RollbackText, RollbackSlot, Position == TableSlotPosition.Rollback, fill);

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

        private static void ApplySlot(TextBlock text, Border slot, bool isActive, Brush fill)
        {
            text.Foreground = isActive ? ActiveText : InactiveText;
            text.FontWeight = isActive ? FontWeights.Bold : FontWeights.Normal;
            slot.Background = isActive ? fill : Brushes.Transparent;
            slot.Visibility = isActive ? Visibility.Visible : Visibility.Collapsed;
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
