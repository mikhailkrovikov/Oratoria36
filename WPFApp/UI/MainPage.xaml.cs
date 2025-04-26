using Oratoria36.Models;
using Oratoria36.Models.Devices;
using Oratoria36.Models.Modules;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.Service;
using Oratoria36.Service.Enums;
using Oratoria36.UI.DialogWindows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = new MainPageVM();
        }
    }

    public class MainPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}