using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Oratoria.Domain.Settings;

namespace Oratoria.UI.ViewModels;

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
