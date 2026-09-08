using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Oratoria.UI.Services;

namespace Oratoria.UI.Views.Pages.DevicePages;

public partial class DeviceMenuPage : Page
{
    public DeviceMenuPage(Frame host, params object[] sources)
    {
        InitializeComponent();
        DataContext = new DeviceMenuPageVM(host, sources);
    }
}

public class DeviceMenuPageVM
{
    public ObservableCollection<DeviceMenuItemVM> Devices { get; } = new();

    public DeviceMenuPageVM(Frame host, params object[] sources)
    {
        foreach (var device in DeviceReflection.CollectDevices(sources))
            Devices.Add(new DeviceMenuItemVM(device, host));
    }
}

public class DeviceMenuItemVM
{
    public string DisplayName { get; }
    public ICommand OpenCommand { get; }

    public DeviceMenuItemVM(object device, Frame host)
    {
        var nameProp = device.GetType().GetProperty("DeviceName");
        DisplayName = nameProp?.GetValue(device)?.ToString() ?? device.GetType().Name;
        OpenCommand = new RelayCommand(_ => host.Navigate(new DevicePage(device)));
    }
}
