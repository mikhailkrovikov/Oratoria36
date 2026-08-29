using Oratoria.Application.Module2;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Module2Context context)
        {
            var v = context.FK_KN_DU_63;
            InitializeComponent();
        }
    }
}