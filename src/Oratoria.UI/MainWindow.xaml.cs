using Oratoria.Domain.Devices;
using Oratoria.UI.Controls.DialogWindows;
using Oratoria.UI.Services;
using Oratoria.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Oratoria.UI;

public partial class MainWindow : Window
{
    private readonly MainWindowVM _vm;

    public MainWindow(
        MainWindowVM maimwindowVM,
        IServiceProvider services)
    {
        InitializeComponent();
        _vm = maimwindowVM;
        DataContext = _vm;
        _vm.StartClock();
        NavigationBarControl.HostFrame = MainFrame;
        NavigationBarControl.PageFactory = type =>
            services.GetService(type) as Page
            ?? (Page)Activator.CreateInstance(type)!;
        NavigationBarControl.Apply(_vm.Navigation);
    }

    private const double MinimumBottomPanelHeight = 158;

    private void BottomPanelResize_DragDelta(object sender, DragDeltaEventArgs e)
    {
        SetBottomPanelHeight(BottomPanel.ActualHeight - e.VerticalChange);
    }

    private void BottomPanelHost_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetBottomPanelHeight(BottomPanel.Height);
    }

    private void SetBottomPanelHeight(double height)
    {
        var maximumHeight = BottomPanelHost.ActualHeight;
        BottomPanel.Height = Math.Clamp(height,
            Math.Min(MinimumBottomPanelHeight, maximumHeight), maximumHeight);
    }

    private void ShowLogs_Click(object sender, RoutedEventArgs e)
    {
        LogGrid.ItemsSource = _vm.Logs;
        LogGrid.Visibility = Visibility.Visible;
        ErrorsListBox.Visibility = Visibility.Collapsed;
    }

    private void ShowErrors_Click(object sender, RoutedEventArgs e)
    {
        LogGrid.Visibility = Visibility.Collapsed;
        ErrorsListBox.Visibility = Visibility.Visible;
    }

    private void ErrorsListBox_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
    {
        if (ErrorsListBox.SelectedItem is not AlarmItem alarm)
            return;

        if (!alarm.HasDescription)
            return;

        var type = alarm.Category switch
        {
            DeviceErrorCategory.Warn => MBType.Warning,
            DeviceErrorCategory.Error => MBType.Error,
            DeviceErrorCategory.Fatal => MBType.Error,
            _ => MBType.Info
        };

        UserMessageBox.Show(
            alarm.Description!,
            alarm.Text,
            type);

        ErrorsListBox.SelectedIndex = -1;
    }
}
