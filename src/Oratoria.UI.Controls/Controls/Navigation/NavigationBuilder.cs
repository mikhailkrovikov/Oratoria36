using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Oratoria.UI.Controls.Controls.Navigation
{
    public class NavigationBuilder
    {
        private readonly List<NavigationItem> _items = new();

        public NavigationBuilder AddSubButton<TPage>(string title) where TPage : Page
        {
            _items.Add(new NavigationItem { Title = title, PageType = typeof(TPage) });
            return this;
        }

        public NavigationBuilder AddMainButton(string title, Action<NavigationBuilder> children)
        {
            var inner = new NavigationBuilder();
            children(inner);
            _items.Add(new NavigationItem { Title = title, Children = inner.Build() });
            return this;
        }

        public IReadOnlyList<NavigationItem> Build() => _items;
    }
}
