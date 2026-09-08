using System.Windows;
using System.Windows.Controls;
using Oratoria.UI.ViewModels;
using Oratoria.UI.Views.Pages.DevicePages;

namespace Oratoria.UI.Views.Pages.Module2Pages
{
    public partial class Module2MnemoPage : Page
    {
        public Module2MnemoPage(Module2MnemoPageVM vm)
        {
            vm.OpenDevice = OpenDevice;
            InitializeComponent();
            DataContext = vm;
            DeviceHost.Navigated += (_, _) => StretchHostContent();
            DeviceHost.SizeChanged += (_, _) => StretchHostContent();
        }

        private void OpenDevice(object device)
        {
            DeviceHost.Navigate(new DevicePage(device));
        }

        private void StretchHostContent()
        {
            if (DeviceHost.Content is not FrameworkElement content)
                return;

            content.Width = DeviceHost.ActualWidth;
            content.Height = DeviceHost.ActualHeight;
        }
    }
}
