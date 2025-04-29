using NLog;
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
    /// Логика взаимодействия для RRGWindow.xaml
    /// </summary>
    public partial class RRGWindow : Window
    {
        RRGWindowVM _vm;
        public RRGWindow(RRG rrg)
        {
            InitializeComponent();
            _vm = new RRGWindowVM(rrg);
            DataContext = _vm;
            _vm.RRG = rrg;
            this.ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class RRGWindowVM
    {
        Logger _logger = LogManager.GetLogger("UI");
        public RRG RRG { get; set; }

        private string _newSetpoint;
        public string NewSetPoint
        {
            get => _newSetpoint;
            set => _newSetpoint = value;
        }
        public ICommand SetNewPoint { get; set; }
        public ICommand Reset { get; set; }
        public string Status
        {
            get
            {
                if (RRG.State == State.On)
                    return "открыт";
                else if (RRG.State == State.Off)
                    return "закрыт";
                else if (RRG.State == State.Transition)
                    return "переходное";
                else if (RRG.State == State.Warning)
                    return "предупреждение";
                else return "ошибка";
            }
        }
        public RRGWindowVM(RRG rrg)
        {
            RRG = rrg;
            SetNewPoint = new RelayCommand((object obj) =>
            {
                try
                {
                    RRG.RRGSetPointSignal.Value = ushort.Parse(NewSetPoint);
                }
                catch { }
            },
            (object obj) =>
            {
                return true;
            });

            Reset = new RelayCommand((object obj) =>
            {
                RRG.RRGSetPointSignal.Value = 0;
            },
            (object obj) =>
            {
                return true;
            });
        }
    }
}
