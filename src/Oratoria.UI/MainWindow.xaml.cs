using Oratoria.Application.Module2;
using Oratoria.Application.TransportModule;
using Oratoria.Application.VacuumModule;
using Oratoria.UI.ViewModels;
using System.Windows;

namespace UI;

public partial class MainWindow : Window
{
    private readonly MainWindowVM _vm;

    public MainWindow(Module2Context module2Context, VacuumContext vacuumContext, TransportContext context)
    {
        InitializeComponent();
        _vm = new MainWindowVM();
        DataContext = _vm;
        _vm.StartClock();
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
}
