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
        Module2Signals _signals;

        public Valve Valve1 { get; set; }
        public ICommand Valve1Command { get; set; }

        public MainPageVM()
        {
            _signals = MainContext.Instance.Module2Signals;

            Valve1Command = new RelayCommand((object obj) => { ValveWindow valveWindow = new(Valve1); });
            Valve1 = new Valve("ФК-КН ДУ-63",
                _signals.DISignals.FK_KN_DU_63_otkryt,
                _signals.DISignals.FK_KN_DU_63_zakryt,
                _signals.DOSignals.FK_KN_otkryt,
                null,
                Valve1Command);


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}