﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Oratoria36.Models;

namespace Oratoria36.UI
{
    public partial class MainWindow : Window
    {
        private readonly ModuleConfig _modbusDevice;
        private readonly MainWindowVM _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            _modbusDevice = new ModuleConfig();
            DataContext = this;
            _vm.StartClock();
            NavigationBarControl.PageChanged += NavigateToPage;
            MainFrame.NavigationService.Navigate(new MainPage());
        }

        public ModuleConfig ModbusDevice => _modbusDevice;
        public MainWindowVM ViewModel => _vm;

        private void NavigateToPage(string pageName)
        {
            switch (pageName)
            {
                case "MainPage":
                    MainFrame.Navigate(new MainPage());
                    break;
                case "SignalsPage":
                    MainFrame.Navigate(new SignalsPage());
                    break;
                case "ConnectionSettings":
                    MainFrame.Navigate(new ConnectionSettings());
                    break;
                case "LogPage":
                    MainFrame.Navigate(new LogPage());
                    break;
                default:
                    throw new ArgumentException($"Unknown page: {pageName}");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NavigationBarControl.PageChanged -= NavigateToPage;
            _modbusDevice?.CloseConnection();
        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
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
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToString("D");
            Time = DateTime.Now.ToString("HH:mm:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}