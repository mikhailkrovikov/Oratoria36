using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NLog;
using Oratoria36.Models;
using Oratoria36.Models.Connection;
using Oratoria36.Service;
using Oratoria36.UI.ModulePages.Module2;
using Oratoria36.UI.Signals;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;
        private readonly MainContext _context;
        private MediaPlayer _mediaPlayer;
        private Popup _imagePopup;
        private bool _isJokeActive = false;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = _vm;
            _vm.StartClock();
            NavigationBarControl.HostFrame = MainFrame;
            _context = MainContext.Instance;
            MainFrame.Navigate(new MainPage());
            _mediaPlayer = new MediaPlayer();
            SetupImagePopup();
            this.MouseDown += MainWindow_MouseDown;
        }

        private void SetupImagePopup()
        {
            Image funnyImage = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\Mikhail\Desktop\gits\Oratoria36\WPFApp\Unilogic\feel_quite_hungry.jpeg")),
                Stretch = Stretch.Uniform,
                Width = 500,
                Height = 400
            };

            Border border = new Border
            {
                Child = funnyImage,
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(5),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    ShadowDepth = 5,
                    Opacity = 0.7
                }
            };

            _imagePopup = new Popup
            {
                Child = border,
                Placement = PlacementMode.Center,
                PlacementTarget = this,
                IsOpen = false,
                AllowsTransparency = true,
                StaysOpen = false
            };
        }

        public MainWindowVM ViewModel => _vm;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_isJokeActive)
            {
                _isJokeActive = true;
                _mediaPlayer.Open(new Uri(@"C:\Users\Mikhail\Desktop\gits\Oratoria36\WPFApp\Unilogic\Tavern music 2 - Kingdom come deliverance 2.mp3"));
                _mediaPlayer.Play();
                _imagePopup.IsOpen = true;
            }
            else
            {
                HideJoke();
            }
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isJokeActive)
            {
                if (!_imagePopup.IsMouseOver)
                {
                    HideJoke();
                }
            }
        }

        private void HideJoke()
        {
            _imagePopup.IsOpen = false;
            _mediaPlayer.Stop();
            _isJokeActive = false;
        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        public ObservableCollection<LogEntry> Logs => DataGridTarget.LogEntries;

        public MainWindowVM()
        {
            CloseButtonCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        public ICommand CloseButtonCommand { get; }

        private DispatcherTimer _timer;

        private string _date;

        private string _time;

        public string Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }

        public void StartClock()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += UpdateDateTime;
            _timer.Start();
            UpdateDateTime(null, null);
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToString("D");
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
