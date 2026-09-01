using System.Windows.Controls;
using Oratoria.UI.ViewModels;

namespace Oratoria.UI.Views.Pages;

public partial class ConnectionSettingsPage : Page
{
    public ConnectionSettingsPage(ConnectionSettingsVM viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
