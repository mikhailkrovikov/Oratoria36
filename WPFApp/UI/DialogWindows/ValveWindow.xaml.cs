using NLog;
using Oratoria36.Models.Devices;
using Oratoria36.Service;
using Oratoria36.Service.Enums;
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
    public class ValveWindowVM : INotifyPropertyChanged
    {
        Logger _logger = LogManager.GetLogger("UI");
        public Valve Valve { get; set; }
        public string Status
        {
            get
            {
                if (Valve.State == State.On)
                    return "открыт";
                else if (Valve.State == State.Off)
                    return "закрыт";
                else if (Valve.State == State.Transition)
                    return "переходное";
                else if (Valve.State == State.Warning)
                    return "предупреждение";
                else return "ошибка";
            }
        }
        public ICommand OpenValveCommand { get; set; }
        public ICommand CloseValveCommand { get; set; }
        public ValveWindowVM(Valve valve)
        {
            Valve = valve;
            OpenValveCommand = new RelayCommand((object obj) =>
            {
                Valve.Open.Value = true;
                _logger.Info($"Открытие клапана {Valve.Name}");
            },
            (object obj) =>
            {
                return !Valve.Open.Value;
            });

            CloseValveCommand = new RelayCommand((object obj) =>
            {
                Valve.Open.Value = false;
                _logger.Info($"Закрытие клапана {Valve.Name}");
            },
            (object obj) =>
            {
                return Valve.Open.Value;
            });

            Valve.IsOpen.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Status));
            };
            Valve.IsClose.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Status));
            };
            Valve.Open.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Status));
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
