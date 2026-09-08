using System.Windows.Controls;
using Oratoria.UI.ViewModels;

namespace Oratoria.UI.Views.Pages.DevicePages;

public partial class DevicePage : Page
{
    public DevicePage(object device)
    {
        InitializeComponent();
        DataContext = new DevicePageVM(device);
    }
}
