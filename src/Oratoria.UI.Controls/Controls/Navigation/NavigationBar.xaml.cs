using System.Windows;
using System.Windows.Controls;
using Oratoria.UI.Controls.Controls.Navigation;

namespace Oratoria.UI.Controls.Navigation
{
    /// <summary>
    /// Логика взаимодействия для NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        private IReadOnlyList<NavigationItem> _root = [];
        private readonly Dictionary<Type, Page> _cache = new();

        public NavigationBar()
        {
            InitializeComponent();
        }

        public Frame? HostFrame {  get; set; }

        public Func<Type, Page> PageFactory { get; set; } = type => (Page)Activator.CreateInstance(type)!;

        public void Apply(IReadOnlyList<NavigationItem> items)
        {
            _root = items;
            RebuildRoot();
            ChildPanel.Children.Clear();
        }

        private void RebuildRoot()
        {
            RootPanel.Children.Clear();
            foreach (var item in _root)
                RootPanel.Children.Add(CreateButton(item, OnRootClick));
        }

        private void OnRootClick(NavigationItem item)
        {
            if (item.IsGroup)
            {
                ShowChildren(item);
                var first = item.Children.FirstOrDefault(c => c.PageType != null);
                if (first != null)
                    Navigate(first);
                return;
            }
            ChildPanel.Children.Clear();
            Navigate(item);
        }

        private void ShowChildren(NavigationItem group)
        {
            ChildPanel.Children.Clear();
            foreach (var child in group.Children)
                ChildPanel.Children.Add(CreateButton(child, Navigate));
        }

        private void Navigate(NavigationItem item)
        {
            if (item.PageType is null || HostFrame is null)
                return;
            if (!_cache.TryGetValue(item.PageType, out var page))
            {
                page = PageFactory(item.PageType);
                _cache[item.PageType] = page;
            }
            HostFrame.Navigate(page);
        }

        private Button CreateButton(NavigationItem item, Action<NavigationItem> onClick)
        {
            var button = new Button
            {
                Content = item.Title,
                Tag = item,
                Style = TryFindResource("Button.Nav") as Style
            };
            button.Click += (_, _) => onClick(item);
            return button;
        }
    }
}
