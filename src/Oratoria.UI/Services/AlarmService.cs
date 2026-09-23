using Oratoria.Domain.Devices;
using Oratoria.Domain.Devices.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Oratoria.UI.Services;

public sealed class AlarmService
{
    private readonly List<object> _devices = new();

    public ObservableCollection<AlarmItem> Items { get; } = new();

    public void AddContext(object context)
    {
        var deviceProperties = context
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var property in deviceProperties)
        {
            var device = property.GetValue(context);

            if (device == null)
                continue;

            var errorsProperty = device
                .GetType()
                .GetProperty("DeviceErrors");

            if (errorsProperty == null)
                continue;

            var errors = errorsProperty.GetValue(device);

            if (errors is not INotifyPropertyChanged propertyChanged)
                continue;

            _devices.Add(device);
            propertyChanged.PropertyChanged += (_, _) => Refresh();
        }

        Refresh();
    }

    private void Refresh()
    {
        if (System.Windows.Application.Current.Dispatcher.CheckAccess())
        {
            RefreshCore();
            return;
        }

        System.Windows.Application.Current.Dispatcher.Invoke(RefreshCore);
    }

    private void RefreshCore()
    {
        Items.Clear();

        foreach (var device in _devices)
        {
            var deviceName = device
                .GetType()
                .GetProperty("DeviceName")?
                .GetValue(device)?
                .ToString();

            var errors = device
                .GetType()
                .GetProperty("DeviceErrors")?
                .GetValue(device) as IEnumerable;

            if (deviceName == null || errors == null)
                continue;

            foreach (var errorObject in errors)
            {
                if (errorObject is not Enum error)
                    continue;

                var field = error
                    .GetType()
                    .GetField(error.ToString());

                var category = field?
                    .GetCustomAttribute<DeviceErrorCategoryAttribute>()?
                    .Category ?? DeviceErrorCategory.None;

                Items.Add(new AlarmItem(
                    deviceName,
                    error,
                    category));
            }
        }
    }
}