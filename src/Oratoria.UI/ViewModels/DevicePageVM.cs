using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
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