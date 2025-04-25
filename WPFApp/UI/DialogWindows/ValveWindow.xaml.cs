using Oratoria36.Models.Devices;
using Oratoria36.Service;
using Oratoria36.Service.Enums;
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
using System.Windows.Shapes;

namespace Oratoria36.UI.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ValveWindow.xaml
    /// </summary>
    public partial class ValveWindow : Window
    {
        ValveWindowVM _vm;
        public ValveWindow(Valve valve)
        {
            InitializeComponent();
            _vm = new ValveWindowVM(valve);
            DataContext = _vm;
            _vm.Valve = valve;
            this.ShowDialog();          
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class ValveWindowVM
    {
        public Valve Valve { get; set; }
        public string Status 
        { 
            get
            {
                if(Valve.State == State.On)
                    return "открыт";
                else if (Valve.State == State.Off)
                    return "закрыт";
                else return "переходное";
            } 
        }
        public ICommand OpenValveCommand { get; set; }
        public ICommand CloseValveCommand { get; set; }
        public ValveWindowVM(Valve valve)
        {
            Valve = valve;
            OpenValveCommand = new RelayCommand((object obj) => 
            { 
                Valve.Open.Value=true; 
            });

            CloseValveCommand = new RelayCommand((object obj) =>
            {
                Valve.Open.Value = false;
            });
        }
    }
}
