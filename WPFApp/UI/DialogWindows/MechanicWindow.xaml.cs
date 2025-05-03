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
    /// Логика взаимодействия для ManipulatorWindow.xaml
    /// </summary>
    public partial class MechanicWindow : Window
    {
        MechanicWindowVM _vm;
        public MechanicWindow(Zatvor zatvor, Manipulator manipulator)
        {
            InitializeComponent();
            this.Show();
            _vm = new MechanicWindowVM(zatvor, manipulator);
            _vm.Manipulator = manipulator;
            _vm.Zatvor = zatvor;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class MechanicWindowVM : INotifyPropertyChanged
    {
        public ICommand Load { get; set; }
        public ICommand Unload { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand HomeToTransportCommand { get; set; }
        public ICommand TransportToHomeCommand { get; set; }
        public ICommand HomeToModuleCommand { get; set; }
        public ICommand ModuleToHomeCommand { get; set; }

        private Zatvor _zatvor;
        public Zatvor Zatvor
        {
            get => _zatvor;
            set
            {
                _zatvor = value;
                OnPropertyChanged(nameof(Zatvor));
            }
        }

        private Manipulator _manipulator;
        public Manipulator Manipulator
        {
            get => _manipulator;
            set
            {
                if (_manipulator != null)
                {
                    // Отписываемся от событий старого манипулятора
                    _manipulator.PropertyChanged -= Manipulator_PropertyChanged;
                    _manipulator.StateChanged -= Manipulator_StateChanged;
                    _manipulator.ErrorStateChanged -= Manipulator_ErrorStateChanged;
                }

                _manipulator = value;

                if (_manipulator != null)
                {
                    // Подписываемся на события нового манипулятора
                    _manipulator.PropertyChanged += Manipulator_PropertyChanged;
                    _manipulator.StateChanged += Manipulator_StateChanged;
                    _manipulator.ErrorStateChanged += Manipulator_ErrorStateChanged;
                }

                OnPropertyChanged(nameof(Manipulator));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(PositionText));
                OnPropertyChanged(nameof(ErrorText));
                OnPropertyChanged(nameof(IsErrorVisible));
            }
        }

        private bool _isWithPlate;
        public bool IsWithPlate
        {
            get => _isWithPlate;
            set
            {
                _isWithPlate = value;
                OnPropertyChanged(nameof(IsWithPlate));
                OnPropertyChanged(nameof(IsWithoutPlate));
                UpdateCommandsCanExecute();
            }
        }

        public bool IsWithoutPlate
        {
            get => !_isWithPlate;
            set
            {
                _isWithPlate = !value;
                OnPropertyChanged(nameof(IsWithPlate));
                OnPropertyChanged(nameof(IsWithoutPlate));
                UpdateCommandsCanExecute();
            }
        }

        public string Status
        {
            get
            {
                if (Manipulator == null) return "";

                switch (Manipulator.State)
                {
                    case State.On:
                        return "в работе";
                    case State.Off:
                        return "в ожидании";
                    case State.Transition:
                        return "перемещается";
                    case State.Warning:
                        return "Предупреждение";
                    case State.Error:
                        return "Ошибка";
                    default:
                        return "";
                }
            }
        }

        public string PositionText
        {
            get
            {
                if (Manipulator == null) return "";

                switch (Manipulator.Position)
                {
                    case ManipulatorPosition.Home:
                        return "Исходная";
                    case ManipulatorPosition.Transport:
                        return "Транспорт";
                    case ManipulatorPosition.Module:
                        return "Модуль";
                    default:
                        return "Неизвестно";
                }
            }
        }

        public string ErrorText
        {
            get
            {
                if (Manipulator == null || Manipulator.ErrorState == ManipulatorErrors.None)
                    return "";

                return Manipulator.ErrorState.ToString();
            }
        }

        public Visibility IsErrorVisible
        {
            get
            {
                if (Manipulator == null || Manipulator.ErrorState == ManipulatorErrors.None)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
        }

        public MechanicWindowVM(Zatvor zatvor, Manipulator manipulator)
        {
            Zatvor = zatvor;
            Manipulator = manipulator;

            // Команды для основных операций
            Load = new RelayCommand(async (object obj) =>
            {
                await manipulator.Load();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None;
            });

            Unload = new RelayCommand(async (object obj) =>
            {
                await manipulator.UnLoad();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None;
            });

            // Команда остановки
            StopCommand = new RelayCommand((object obj) =>
            {
                manipulator.EmergencyStop();
            });

            // Команды для отдельных перемещений
            HomeToTransportCommand = new RelayCommand(async (object obj) =>
            {
                if (IsWithPlate)
                    await manipulator.FromHomeToTransportWithPlate();
                else
                    await manipulator.FromHomeToTransportNoPlate();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None &&
                       (IsWithPlate || IsWithoutPlate);
            });

            TransportToHomeCommand = new RelayCommand(async (object obj) =>
            {
                if (IsWithPlate)
                    await manipulator.FromTransportToHomeWithPlate();
                else
                    await manipulator.FromTransportToHomeNoPlate();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None &&
                       (IsWithPlate || IsWithoutPlate);
            });

            HomeToModuleCommand = new RelayCommand(async (object obj) =>
            {
                if (IsWithPlate)
                    await manipulator.FromHomeToModuleWithPlate();
                else
                    await manipulator.FromHomeToModuleNoPlate();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None &&
                       (IsWithPlate || IsWithoutPlate);
            });

            ModuleToHomeCommand = new RelayCommand(async (object obj) =>
            {
                if (IsWithPlate)
                    await manipulator.FromModuleToHomeWithPlate();
                else
                    await manipulator.FromModuleToHomeNoPlate();
            },
            (object obj) =>
            {
                return manipulator.ErrorState == ManipulatorErrors.None &&
                       (IsWithPlate || IsWithoutPlate);
            });
        }

        private void Manipulator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Manipulator.Position))
            {
                OnPropertyChanged(nameof(PositionText));
            }
        }

        private void Manipulator_StateChanged(object sender, State e)
        {
            OnPropertyChanged(nameof(Status));
            UpdateCommandsCanExecute();
        }

        private void Manipulator_ErrorStateChanged(object sender, ManipulatorErrors e)
        {
            OnPropertyChanged(nameof(ErrorText));
            OnPropertyChanged(nameof(IsErrorVisible));
            UpdateCommandsCanExecute();
        }

        private void UpdateCommandsCanExecute()
        {
            // Обновляем состояние CanExecute для всех команд
            (Load as RelayCommand)?.RaiseCanExecuteChanged();
            (Unload as RelayCommand)?.RaiseCanExecuteChanged();
            (HomeToTransportCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (TransportToHomeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (HomeToModuleCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ModuleToHomeCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
