using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Oratoria.Domain.Devices;
using Oratoria.Infrastructure;

namespace Oratoria.UI.ViewModels;

internal sealed class ActionParameterBinding
{
    private readonly Type _parameterType;
    private TextBox? _textBox;
    private CheckBox? _checkBox;
    private ComboBox? _comboBox;

    public FrameworkElement? Control { get; }

    public ActionParameterBinding(ParameterInfo parameter)
    {
        _parameterType =
            Nullable.GetUnderlyingType(parameter.ParameterType)
            ?? parameter.ParameterType;

        if (_parameterType == typeof(CancellationToken))
            return;

        var label = parameter
            .GetCustomAttribute<DeviceActionParameterAttribute>()?.Name;

        Control = Wrap(CreateEditor(), label);
    }

    public static bool IsSupported(Type type)
    {
        if (type == typeof(CancellationToken))
            return true;

        type = Nullable.GetUnderlyingType(type) ?? type;

        return type == typeof(bool)
            || type.IsEnum
            || IsNumeric(type);
    }

    public object? GetValue()
    {
        if (_parameterType == typeof(CancellationToken))
            return CancellationToken.None;
        if (_textBox != null)
        {
            try
            {
                return Convert.ChangeType(_textBox.Text, _parameterType, CultureInfo.CurrentCulture);
            }
            catch
            {
                return _parameterType.IsValueType ? Activator.CreateInstance(_parameterType) : null;
            }
        }

        if (_checkBox != null)
            return _checkBox.IsChecked ?? false;

        if (_comboBox != null && _comboBox.SelectedIndex >= 0)
            return Enum.GetValues(_parameterType).GetValue(_comboBox.SelectedIndex);

        return _parameterType.IsValueType ? Activator.CreateInstance(_parameterType) : null;
    }

    private FrameworkElement CreateEditor()
    {
        if (IsNumeric(_parameterType))
        {
            _textBox = new TextBox
            {
                Text = "0",
                Width = 110,
                Height = 26,
                FontSize = 16,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            _textBox.SetResourceReference(FrameworkElement.StyleProperty, "TextBoxInput");
            return _textBox;
        }

        if (_parameterType == typeof(bool))
        {
            _checkBox = new CheckBox { IsChecked = false, VerticalAlignment = VerticalAlignment.Center };
            return _checkBox;
        }

        _comboBox = new ComboBox
        {
            Width = 110,
            Height = 26,
            FontSize = 16,
            ItemsSource = Enum.GetValues(_parameterType).Cast<Enum>().Select(e => e.GetDescription()).ToList(),
            SelectedIndex = 0,
            VerticalAlignment = VerticalAlignment.Center
        };
        return _comboBox;
    }

    private static FrameworkElement Wrap(FrameworkElement editor, string? label)
    {
        if (string.IsNullOrWhiteSpace(label))
            return editor;

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center
        };
        panel.Children.Add(editor);
        var caption = new TextBlock
        {
            Text = label,
            Margin = new Thickness(8, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 14
        };
        caption.SetResourceReference(FrameworkElement.StyleProperty, "Text.Body");
        panel.Children.Add(caption);
        return panel;
    }

    private static bool IsNumeric(Type type)
    {
        return Type.GetTypeCode(type) is
            TypeCode.Byte or TypeCode.SByte or
            TypeCode.Int16 or TypeCode.UInt16 or
            TypeCode.Int32 or TypeCode.UInt32 or
            TypeCode.Int64 or TypeCode.UInt64 or
            TypeCode.Single or TypeCode.Double or TypeCode.Decimal;
    }
}
