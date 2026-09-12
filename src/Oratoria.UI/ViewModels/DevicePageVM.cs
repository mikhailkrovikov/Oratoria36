using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Oratoria.Domain.Devices;
using Oratoria.Domain.Settings;
using Oratoria.Domain.Signals;
using Oratoria.Infrastructure;
using Oratoria.UI.Logging;
using Oratoria.UI.Services;

namespace Oratoria.UI.ViewModels;

public class DevicePageVM : INotifyPropertyChanged
{
    private readonly object _device;
    private readonly Dispatcher _dispatcher;
    private readonly PropertyInfo _deviceNameProp;
    private readonly PropertyInfo _stateProp;
    private readonly object _deviceErrors;
    private readonly Type _errorEnumType;
    private readonly MethodInfo _getCategoryMethod;
    private readonly MethodInfo? _mapErrorMethod;
    private readonly CollectionViewSource _deviceLogsSource;
    private bool _isRunning;

    public string DeviceName => _deviceNameProp.GetValue(_device)?.ToString() ?? _device.GetType().Name;

    public string Status
    {
        get
        {
            var state = _stateProp.GetValue(_device);
            return state is Enum e ? e.GetDescription() : string.Empty;
        }
    }

    public ObservableCollection<string> Errors { get; } = new();
    public bool HasErrors => Errors.Count > 0;
    public ObservableCollection<DeviceActionVM> Actions { get; } = new();
    public ObservableCollection<DeviceSettingItemVM> Settings { get; } = new();
    public ObservableCollection<DeviceSignalItemVM> InputSignals { get; } = new();
    public ObservableCollection<DeviceSignalItemVM> OutputSignals { get; } = new();
    public ICollectionView DeviceLogs { get; }

    public DevicePageVM(object device)
    {
        _device = device ?? throw new ArgumentNullException(nameof(device));
        _dispatcher = Dispatcher.CurrentDispatcher;

        var deviceInterface = DeviceReflection.GetDeviceInterface(device.GetType());

        _deviceNameProp = device.GetType().GetProperty("DeviceName");
        _stateProp = device.GetType().GetProperty("Position")
            ?? deviceInterface.GetProperty("State")!;
        _deviceErrors = deviceInterface.GetProperty("DeviceErrors")!.GetValue(device)!;
        _errorEnumType = deviceInterface.GetGenericArguments()[1];
        _mapErrorMethod = device.GetType().GetMethod("MapError", new[] { _errorEnumType });
        _getCategoryMethod = typeof(DeviceError<>).MakeGenericType(_errorEnumType).GetMethod(nameof(DeviceError<Enum>.GetCategory))!;

        var stateChanged = deviceInterface.GetEvent("StateChanged")!;
        var stateHandler = Delegate.CreateDelegate(
            stateChanged.EventHandlerType!,
            this,
            GetType().GetMethod(nameof(OnStateChanged), BindingFlags.NonPublic | BindingFlags.Instance)!);
        stateChanged.AddEventHandler(_device, stateHandler);

        var errorChanged = _deviceErrors.GetType().GetEvent("ErrorChanged")!;
        var errorHandlerMethod = GetType()
            .GetMethod(nameof(OnErrorChanged), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(_errorEnumType);
        var errorHandler = Delegate.CreateDelegate(errorChanged.EventHandlerType!, this, errorHandlerMethod);
        errorChanged.AddEventHandler(_deviceErrors, errorHandler);

        if (_deviceErrors is INotifyPropertyChanged errorProps)
            errorProps.PropertyChanged += (_, _) => RunOnUi(RefreshErrors);

        BuildActions();
        BuildSettings();
        BuildSignals();
        RefreshErrors();

        _deviceLogsSource = new CollectionViewSource { Source = DataGridTarget.LogEntries };
        DeviceLogs = _deviceLogsSource.View;
        DeviceLogs.Filter = obj => obj is LogEntry entry && entry.Logger == DeviceName;
    }

    private void BuildActions()
    {
        var methods = _device.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Select(m => (Method: m, Attr: m.GetCustomAttribute<DeviceActionAttribute>()))
            .Where(x => x.Attr != null && IsSupportedActionReturnType(x.Method.ReturnType))
            .ToList();

        foreach (var (method, attr) in methods)
        {
            var hiddenByDerived = methods.Any(other =>
                other.Method != method
                && other.Attr!.Name == attr!.Name
                && method.DeclaringType != null
                && other.Method.DeclaringType != null
                && method.DeclaringType != other.Method.DeclaringType
                && method.DeclaringType.IsAssignableFrom(other.Method.DeclaringType));

            if (hiddenByDerived)
                continue;

            if (!DeviceActionVM.CanBuild(method))
                continue;

            Actions.Add(new DeviceActionVM(
                _device,
                method,
                attr!,
                () => _isRunning,
                SetRunning));
        }
    }

    private static bool IsSupportedActionReturnType(Type returnType)
    {
        if (returnType == typeof(void) || returnType == typeof(Task))
            return true;

        return returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>);
    }

    private void BuildSettings()
    {
        foreach (var prop in _device.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetIndexParameters().Length != 0)
                continue;

            object? value;
            try
            {
                value = prop.GetValue(_device);
            }
            catch
            {
                continue;
            }

            if (value is Setting setting)
                Settings.Add(new DeviceSettingItemVM(setting, _dispatcher));
        }
    }

    private void BuildSignals()
    {
        foreach (var prop in _device.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetIndexParameters().Length != 0)
                continue;

            object? value;
            try
            {
                value = prop.GetValue(_device);
            }
            catch
            {
                continue;
            }

            if (value == null)
                continue;

            if (!TryGetSignalValueType(value.GetType(), out var genericDef, out var valueType))
                continue;

            var item = new DeviceSignalItemVM(value, valueType, _dispatcher);
            if (genericDef == typeof(InputSignal<>))
                InputSignals.Add(item);
            else if (genericDef == typeof(OutputSignal<>))
                OutputSignals.Add(item);
        }
    }

    private static bool TryGetSignalValueType(Type type, out Type genericDef, out Type valueType)
    {
        var current = type;
        while (current != null && current != typeof(object))
        {
            if (current.IsGenericType)
            {
                var def = current.GetGenericTypeDefinition();
                if (def == typeof(InputSignal<>) || def == typeof(OutputSignal<>))
                {
                    genericDef = def;
                    valueType = current.GetGenericArguments()[0];
                    return true;
                }
            }

            current = current.BaseType;
        }

        genericDef = null!;
        valueType = null!;
        return false;
    }

    private void SetRunning(bool value)
    {
        _isRunning = value;
        CommandManager.InvalidateRequerySuggested();
    }

    private void OnStateChanged()
    {
        RunOnUi(() =>
        {
            OnPropertyChanged(nameof(Status));
            CommandManager.InvalidateRequerySuggested();
        });
    }

    private void OnErrorChanged<T>(T error) where T : Enum => RunOnUi(RefreshErrors);

    private void RefreshErrors()
    {
        Errors.Clear();
        foreach (var item in (IEnumerable)_deviceErrors)
        {
            if (item is not Enum error)
                continue;

            if (Equals(error, Activator.CreateInstance(error.GetType())) && error.ToString() == "None")
                continue;

            var category = (DeviceErrorCategory)_getCategoryMethod.Invoke(null, new object[] { error })!;
            if (category == DeviceErrorCategory.None)
                continue;

            var displayError = _mapErrorMethod is null
                ? error
                : (Enum)_mapErrorMethod.Invoke(_device, new object[] { error })!;
            Errors.Add(displayError.GetDescription());
        }

        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(Status));
    }

    private void RunOnUi(Action action)
    {
        if (_dispatcher.CheckAccess())
            action();
        else
            _dispatcher.BeginInvoke(action);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class DeviceActionVM
{
    private readonly object _device;
    private readonly MethodInfo _method;
    private readonly Func<bool> _isRunning;
    private readonly Action<bool> _setRunning;
    private readonly List<ActionParameterBinding> _parameters = new();

    public string DisplayName { get; }
    public ICommand Command { get; }
    public FrameworkElement? ParameterControl { get; }

    public DeviceActionVM(
        object device,
        MethodInfo method,
        DeviceActionAttribute action,
        Func<bool> isRunning,
        Action<bool> setRunning)
    {
        _device = device;
        _method = method;
        _isRunning = isRunning;
        _setRunning = setRunning;
        DisplayName = action.Name;
        ParameterControl = CreateParameterControl(method.GetParameters());
        Command = new RelayCommand(
            async _ =>
            {
                _setRunning(true);
                try
                {
                    var args = GetArgs();
                    var result = _method.Invoke(_device, args);
                    if (result is Task task)
                        await task;
                }
                finally
                {
                    _setRunning(false);
                }
            },
            _ => !_isRunning() && CanExecute());
    }

    public static bool CanBuild(MethodInfo method)
    {
        foreach (var parameter in method.GetParameters())
        {
            if (!ActionParameterBinding.IsSupported(parameter.ParameterType))
                return false;
        }

        return true;
    }

    private FrameworkElement? CreateParameterControl(ParameterInfo[] parameters)
    {
        if (parameters.Length == 0)
            return null;

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center
        };

        foreach (var parameter in parameters)
        {
            var binding = new ActionParameterBinding(parameter);
            _parameters.Add(binding);
            if (binding.Control != null)
                panel.Children.Add(binding.Control);
        }

        return panel;
    }

    private object?[] GetArgs() => _parameters.Select(p => p.GetValue()).ToArray();

    private bool CanExecute()
    {
        var canMethod = _device.GetType().GetMethod("Can" + _method.Name, BindingFlags.Public | BindingFlags.Instance);
        if (canMethod == null || canMethod.GetParameters().Length != 0 || canMethod.ReturnType != typeof(bool))
            return true;

        return (bool)canMethod.Invoke(_device, null)!;
    }
}

internal sealed class ActionParameterBinding
{
    private readonly Type _parameterType;
    private TextBox? _textBox;
    private CheckBox? _checkBox;
    private ComboBox? _comboBox;

    public FrameworkElement Control { get; }

    public ActionParameterBinding(ParameterInfo parameter)
    {
        _parameterType = Nullable.GetUnderlyingType(parameter.ParameterType) ?? parameter.ParameterType;
        var label = parameter.GetCustomAttribute<DeviceActionParameterAttribute>()?.Name;
        Control = Wrap(CreateEditor(), label);
    }

    public static bool IsSupported(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        return type == typeof(bool) || type.IsEnum || IsNumeric(type);
    }

    public object? GetValue()
    {
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

public class DeviceSettingItemVM : INotifyPropertyChanged
{
    private readonly Setting _setting;
    private readonly PropertyInfo? _valueProp;
    private readonly Dispatcher _dispatcher;

    public string DisplayName { get; }
    public string Unit { get; }
    public string DisplayNameWithUnit => string.IsNullOrWhiteSpace(Unit) ? DisplayName : $"{DisplayName}, {Unit}";
    public string RangeText { get; }

    public string ValueText
    {
        get => _valueProp?.GetValue(_setting)?.ToString() ?? string.Empty;
        set
        {
            if (_valueProp == null)
                return;

            try
            {
                var converted = Convert.ChangeType(value, _valueProp.PropertyType, CultureInfo.CurrentCulture);
                _valueProp.SetValue(_setting, converted);
                OnPropertyChanged();
            }
            catch
            {
                OnPropertyChanged();
            }
        }
    }

    public DeviceSettingItemVM(Setting setting, Dispatcher dispatcher)
    {
        _setting = setting;
        _dispatcher = dispatcher;
        DisplayName = setting.Name;
        Unit = setting.Unit;
        _valueProp = setting.GetType().GetProperty("Value");
        RangeText = FormatRange(setting);

        setting.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is null or "Value")
                RunOnUi(() => OnPropertyChanged(nameof(ValueText)));
        };
    }

    private static string FormatRange(Setting setting)
    {
        var min = setting.GetType().GetProperty("MinValue")?.GetValue(setting);
        var max = setting.GetType().GetProperty("MaxValue")?.GetValue(setting);
        if (min == null && max == null)
            return "—";
        if (min == null)
            return $"≤ {max}";
        if (max == null)
            return $"≥ {min}";
        return $"{min} - {max}";
    }

    private void RunOnUi(Action action)
    {
        if (_dispatcher.CheckAccess())
            action();
        else
            _dispatcher.BeginInvoke(action);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class DeviceSignalItemVM : INotifyPropertyChanged
{
    private readonly object _signal;
    private readonly PropertyInfo _valueProp;
    private readonly Type _valueType;
    private readonly Dispatcher _dispatcher;
    private bool _updating;

    public string DisplayName { get; }
    public bool IsBool { get; }
    public Visibility BoolVisibility => IsBool ? Visibility.Visible : Visibility.Collapsed;
    public Visibility TextVisibility => IsBool ? Visibility.Collapsed : Visibility.Visible;

    public bool BoolValue
    {
        get => IsBool && (bool)_valueProp.GetValue(_signal)!;
        set
        {
            if (!IsBool || _updating)
                return;
            WriteSignalValue(value);
        }
    }

    public string TextValue
    {
        get => IsBool ? string.Empty : _valueProp.GetValue(_signal)?.ToString() ?? string.Empty;
        set
        {
            if (IsBool || _updating)
                return;
            try
            {
                WriteSignalValue(Convert.ChangeType(value, _valueType, CultureInfo.CurrentCulture));
            }
            catch
            {
                OnPropertyChanged();
            }
        }
    }

    public DeviceSignalItemVM(object signal, Type valueType, Dispatcher dispatcher)
    {
        _signal = signal;
        _valueType = valueType;
        _dispatcher = dispatcher;
        _valueProp = signal.GetType().GetProperty("Value")!;
        IsBool = valueType == typeof(bool);

        var nameProp = signal.GetType().GetProperty("Name");
        DisplayName = nameProp?.GetValue(signal)?.ToString() ?? string.Empty;

        if (signal is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName is not (null or "Value"))
                    return;

                RunOnUi(() =>
                {
                    _updating = true;
                    try
                    {
                        OnPropertyChanged(nameof(BoolValue));
                        OnPropertyChanged(nameof(TextValue));
                    }
                    finally
                    {
                        _updating = false;
                    }
                });
            };
        }
    }

    private void WriteSignalValue(object? value)
    {
        Task.Run(() =>
        {
            try
            {
                _valueProp.SetValue(_signal, value);
            }
            catch
            {
                RunOnUi(() =>
                {
                    OnPropertyChanged(nameof(BoolValue));
                    OnPropertyChanged(nameof(TextValue));
                });
            }
        });
    }

    private void RunOnUi(Action action)
    {
        if (_dispatcher.CheckAccess())
            action();
        else
            _dispatcher.BeginInvoke(action);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
