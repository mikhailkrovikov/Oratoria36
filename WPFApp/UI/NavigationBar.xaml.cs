using System;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI
{
    public partial class NavigationBar : UserControl
    {
        public event Action<string> PageChanged;

        public NavigationBar()
        {
            InitializeComponent();
        }
        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke("MainPage");
        }
        private void SignalsPage_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke("Module2SignalsPage");
            
        }
        private void ConnectionSettings_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke("ConnectionSettings");
        }
        private void LogPage_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke("LogPage"); 
        }
        private void Module2page_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke("Module2Page");
        }
    }
}
