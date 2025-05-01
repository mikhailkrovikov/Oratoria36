using Oratoria36.UI.ModulePages.Module2;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using Oratoria36.UI.Signals;

namespace Oratoria36.UI
{
    public partial class NavigationBar : UserControl
    {
        private Frame _hostFrame;
        string _currentModule;

        Dictionary<string, Page> _module2Pages;

        public Frame HostFrame
        {
            get => _hostFrame;
            set
            {
                _hostFrame = value;
                if (_hostFrame != null)
                {
                    InitializeStaticButtons();
                }
            }
        }

        public NavigationBar()
        {
            InitializeComponent();

            _module2Pages = new Dictionary<string, Page>
            {
                { "Мнемосхема", new Module2Page() },
                { "Сигналы", new Module2SignalsPage() },
                { "Логи", new LogPage() }
            };
        }

        private void InitializeStaticButtons()
        {
            if (HostFrame == null) return;

            StaticPanel.Children.Clear();

            var buttons = new Dictionary<string, object>
            {
                { "Главная", new MainPage() },
                { "Модуль 2", "Module2" },
                { "Сеть", new ConnectionSettings() }
            };

            foreach (var button in buttons)
            {
                var btn = new Button()
                {
                    Content = button.Key,
                    Style = (Style)Application.Current.FindResource("NavigationButton"),
                    Tag = button.Value
                };

                btn.Click += StaticButton_Click;
                StaticPanel.Children.Add(btn);
            }
        }

        private void StaticButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is string moduleId)
                {
                    ShowModulePages(moduleId);
                    if (moduleId == "Module2")
                    {
                        HostFrame.Navigate(_module2Pages["Мнемосхема"]);
                    }
                }
                else if (button.Tag is Page page)
                {
                    HostFrame.Navigate(page);

                    if (_currentModule != null)
                    {
                        DynamicPanel.Children.Clear();
                        _currentModule = null;
                    }
                }
            }
        }

        private void ShowModulePages(string moduleId)
        {
            _currentModule = moduleId;
            DynamicPanel.Children.Clear();

            if (moduleId == "Module2")
            {
                foreach (var page in _module2Pages)
                {
                    var btn = new Button()
                    {
                        Content = page.Key,
                        Style = (Style)Application.Current.FindResource("NavigationButton"),
                        Tag = page.Value
                    };

                    btn.Click += DynamicButton_Click;
                    DynamicPanel.Children.Add(btn);
                }
            }
        }

        private void DynamicButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Page page)
            {
                HostFrame.Navigate(page);
            }
        }
    }
}
