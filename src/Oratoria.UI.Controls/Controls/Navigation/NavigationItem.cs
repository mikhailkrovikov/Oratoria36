namespace Oratoria.UI.Controls.Controls.Navigation
{
    public class NavigationItem
    {
        public string Title { get; set; }
        public Type? PageType {  get; set; }
        public IReadOnlyList<NavigationItem> Children { get; set; } = [];
        public bool IsGroup => Children.Count > 0;
    }
}
