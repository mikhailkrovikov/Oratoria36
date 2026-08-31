using Oratoria.Application.Module2;
using Oratoria.Application.TransportModule;
using Oratoria.Application.VacuumModule;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Module2Context module2Context, VacuumContext vacuumContext, TransportContext context)
        {
            InitializeComponent();
        }
    }
}