using Oratoria36.Models.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oratoria36.UI.Signals
{
    /// <summary>
    /// Логика взаимодействия для Module2Signals.xaml
    /// </summary>
    public partial class Module2SignalsPage : Page
    {
        Module2SignalsVM _vm;
        public Module2SignalsPage()
        {
            
            InitializeComponent();
            _vm = new Module2SignalsVM(this);
            DataContext = _vm;
        }
    }
    public class Module2SignalsVM : INotifyPropertyChanged
    {
        Module2Signals _signals;
        public Module2SignalsVM(Module2SignalsPage page)
        {
            _signals = new Module2Signals();
            ConfigInputs(page.DigitalInputGrid);
        }

        public void ConfigInputs(Grid grid)
        {
            int rowIndex = 1; // строка 0 занята заголовками

            foreach (var signal in _signals.DISignals.DigitalInputs)
            {
                // Добавляем строку в грид
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Колонка 0 — № пина
                var pinLabel = new Label
                {
                    Content = signal.PinNumber,
                    Style = (Style)Application.Current.Resources["BaseTextLabel"]
                };
                Grid.SetRow(pinLabel, rowIndex);
                Grid.SetColumn(pinLabel, 0);
                grid.Children.Add(pinLabel);

                // Колонка 1 — Название сигнала
                var nameLabel = new Label
                {
                    Content = signal.Name,
                    Style = (Style)Application.Current.Resources["BaseTextLabel"]
                };
                Grid.SetRow(nameLabel, rowIndex);
                Grid.SetColumn(nameLabel, 1);
                grid.Children.Add(nameLabel);

                // Колонка 2 — Состояние сигнала через ToggleButton
                var stateButton = new ToggleButton
                {
                    Style = (Style)Application.Current.Resources["SignalDisplayToggleStyle"]
                };

                // Привязка к значению сигнала
                stateButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Value")
                {
                    Source = signal,
                    Mode = BindingMode.OneWay
                });

                // Привязка текста (ВКЛ / ВЫКЛ)
                stateButton.SetBinding(ToggleButton.ContentProperty, new Binding("Value")
                {
                    Source = signal,
                    Mode = BindingMode.OneWay,
                    Converter = (IValueConverter)Application.Current.Resources["SignalStateTextConverter"]
                });

                Grid.SetRow(stateButton, rowIndex);
                Grid.SetColumn(stateButton, 2);
                grid.Children.Add(stateButton);

                rowIndex++;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
