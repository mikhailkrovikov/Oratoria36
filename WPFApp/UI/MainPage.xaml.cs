using Oratoria36.Models;
using Oratoria36.Models.Modules;
using Oratoria36.Models.Modules.Module2;
using Oratoria36.Service;
using Oratoria36.Service.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private readonly Module2Signals _signals;

        public ICommand Valve1Command { get; } = new RelayCommand(_ =>
        {
            MainContext.Instance.Module2Signals.DOSignals.Anod_vklyuchit.Value = true;
            MainContext.Instance.Module2Signals.DOSignals.VCH_vyklyuchit.Value = false;
        });

        public ICommand Valve2Command { get; } = new RelayCommand(_ =>
        {
            MainContext.Instance.Module2Signals.DOSignals.VCH_vyklyuchit.Value = true;
            MainContext.Instance.Module2Signals.DOSignals.Anod_vklyuchit.Value = false;
        });



        public MainPage()
        {
            _signals = MainContext.Instance.Module2Signals;

            InitializeComponent();
            DataContext = this;

            _signals.DOSignals.Anod_vklyuchit.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Valve1State));
                OnPropertyChanged(nameof(Valve2State));
            };

            _signals.DOSignals.VCH_vyklyuchit.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Valve1State));
                OnPropertyChanged(nameof(Valve2State));
            };
        }

        public State Valve1State => _signals.DOSignals.Anod_vklyuchit.Value ? State.On : State.Off;
        public State Valve2State => _signals.DOSignals.VCH_vyklyuchit.Value ? State.On : State.Error;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}