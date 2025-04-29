using Oratoria36.Models.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oratoria36.UI.ModulePages.Module2
{
    /// <summary>
    /// Логика взаимодействия для Module2Scheme.xaml
    /// </summary>
    public partial class Module2Scheme : UserControl
    {
        public static DependencyProperty FK_KN_DU_63Property =
            DependencyProperty.Register("FK_KN_DU_63",
            typeof(Valve),
            typeof(Module2Scheme));

        public static DependencyProperty Pipeline1Property =
            DependencyProperty.Register("Pipeline1",
            typeof(Pipeline),
            typeof(Module2Scheme));

        public static DependencyProperty RRG1Property =
            DependencyProperty.Register("RRG1",
            typeof(RRG),
            typeof(Module2Scheme));


        public Valve FK_KN_DU_63
        {
            get { return (Valve)GetValue(FK_KN_DU_63Property); }
            set { SetValue(FK_KN_DU_63Property, value); }
        }
        public Pipeline Pipeline1
        {
            get { return (Pipeline)GetValue(Pipeline1Property); }
            set { SetValue(Pipeline1Property, value); }
        }

        public RRG RRG1
        {
            get { return (RRG)GetValue(RRG1Property); }
            set { SetValue(RRG1Property, value); }
        }
        public Module2Scheme()
        {
            InitializeComponent();
        }
    }
}
