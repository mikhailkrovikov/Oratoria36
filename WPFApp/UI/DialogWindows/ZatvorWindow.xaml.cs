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
    /// Логика взаимодействия для ZatvorWindow.xaml
    /// </summary>
    public partial class ZatvorWindow : Window
    {
        ZatvorWindowVM _vm;
        public ZatvorWindow(Zatvor zatvor)
        {
            InitializeComponent();
            _vm = new ZatvorWindowVM(zatvor);
            DataContext = _vm;
            _vm.Zatvor = zatvor;
            this.ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class ZatvorWindowVM : INotifyPropertyChanged
    {
        Logger _logger = LogManager.GetLogger("UI");
        public Zatvor Zatvor { get; set; }
        public string Status
        {
            get
            {
                if (Zatvor.State == State.On)
                    return "открыт";
                else if (Zatvor.State == State.Off)
                    return "закрыт";
                else if (Zatvor.State == State.Transition)
                    return "переходное";
                else if (Zatvor.State == State.Warning)
                    return "предупреждение";
                else return "ошибка";
            }
        }
        public ICommand OpenZatvorCommand { get; set; }
        public ICommand CloseZatvorCommand { get; set; }
        public ZatvorWindowVM(Zatvor zatvor)
        {
            Zatvor = zatvor;
            OpenZatvorCommand = new RelayCommand((object obj) =>
            {
                Zatvor.Open.Value = true;
                _logger.Info($"Открытие затвора {Zatvor.Name}");
            },
            (object obj) => 
            { 
                return !Zatvor.Open.Value; 
            });

            CloseZatvorCommand = new RelayCommand((object obj) => 
            {
                Zatvor.Open.Value = false;
                _logger.Info($"зкрытие затвора {Zatvor.Name}");
            },
            (object obj) =>
            {
                return Zatvor.Open.Value;
            });

            Zatvor.IsOpen.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Status));
            };
            Zatvor.IsClose.OnSignalChanged += value =>
            {
                OnPropertyChanged(nameof(Status));
            };
            Zatvor.Open.OnSignalChanged += value =>
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
