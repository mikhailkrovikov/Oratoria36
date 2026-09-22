using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Oratoria.UI.ViewModels;

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
