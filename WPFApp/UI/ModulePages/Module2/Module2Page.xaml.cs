using Oratoria36.Models.Devices;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.Models;
using Oratoria36.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Oratoria36.UI.DialogWindows;

namespace Oratoria36.UI.ModulePages.Module2
{
    /// <summary>
    /// Логика взаимодействия для Module2Page.xaml
    /// </summary>
    public partial class Module2Page : Page
    {
        public Module2Page()
        {
            InitializeComponent();
            this.DataContext = new Module2PageVM();
        }
    }
    public class Module2PageVM : INotifyPropertyChanged
    {
        Module2Signals _signals;

        public Valve FK_KN_DU_63 { get; set; }

        public ICommand FK_KN_DU_63Command { get; set; }

        public Module2PageVM()
        {
            _signals = MainContext.Instance.Module2Signals;

            FK_KN_DU_63Command = new RelayCommand(
                (object obj) => { ValveWindow valveWindow = new(FK_KN_DU_63); },
                (object obj) => { return true; });

            FK_KN_DU_63 = new Valve("ФК-КН ДУ-63",
                _signals.DISignals.FK_KN_DU_63_otkryt,
                _signals.DISignals.FK_KN_DU_63_zakryt,
                _signals.DOSignals.FK_KN_otkryt,
                null,
                FK_KN_DU_63Command);


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
