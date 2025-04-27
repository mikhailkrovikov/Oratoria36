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

namespace Oratoria36.UI.UserElements
{
    public partial class GasLineUE : UserControl
    {
        public static readonly DependencyProperty GasNameProperty =
            DependencyProperty.Register("GasName",
                                      typeof(string),
                                      typeof(GasLineUE),
                                      new PropertyMetadata(""));

        public string GasName
        {
            get { return (string)GetValue(GasNameProperty); }
            set { SetValue(GasNameProperty, value); }
        }

        public GasLineUE()
        {
            InitializeComponent();
        }
    }
}
