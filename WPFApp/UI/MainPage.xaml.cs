using Oratoria36.Models.Modules;
using Oratoria36.Service.Enums;
using Oratoria36.UI.UserElements;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Oratoria36.UI
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        bool doit;
       Module2Signals signals;
        public MainPage()
        {
            signals = new Module2Signals();
            InitializeComponent();
            DataContext = this;
            Valve1.Click += (s, e) => { MessageBox.Show("dsd"); doit = true; OnPropertyChanged(nameof(Valve1State)); };
            
        }
        public  State Valve1State
        {
            get
            {
                if(doit)
                    return State.On;
                return State.Error;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void Valve1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Valve2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
