using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Oratoria.UI.Services.Recipe
{
    public static class DataGridColumnsBehavior
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public static void SetItemsSource(DependencyObject obj, IEnumerable value) =>
            obj.SetValue(ItemsSourceProperty, value);

        public static IEnumerable GetItemsSource(DependencyObject obj) =>
            (IEnumerable)obj.GetValue(ItemsSourceProperty);

        private static readonly DependencyProperty IsDynamicProperty =
            DependencyProperty.RegisterAttached(
                "IsDynamic",
                typeof(bool),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(false));

        private static void SetIsDynamic(DependencyObject obj, bool value) =>
            obj.SetValue(IsDynamicProperty, value);

        private static bool GetIsDynamic(DependencyObject obj) =>
            (bool)obj.GetValue(IsDynamicProperty);

        private static readonly DependencyProperty HandlerProperty =
            DependencyProperty.RegisterAttached(
                "Handler",
                typeof(NotifyCollectionChangedEventHandler),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(null));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid grid) return;

            if (e.OldValue is INotifyCollectionChanged oldCol &&
                grid.GetValue(HandlerProperty) is NotifyCollectionChangedEventHandler oldHandler)
            {
                oldCol.CollectionChanged -= oldHandler;
                grid.ClearValue(HandlerProperty);
            }

            if (e.NewValue is INotifyCollectionChanged newCol)
            {
                NotifyCollectionChangedEventHandler handler = (_, _) => RebuildColumns(grid);
                newCol.CollectionChanged += handler;
                grid.SetValue(HandlerProperty, handler);
            }

            RebuildColumns(grid);
        }

        private static void RebuildColumns(DataGrid grid)
        {
            var steps = GetItemsSource(grid) as IList;
            if (steps == null) return;

            for (int i = grid.Columns.Count - 1; i >= 0; i--)
            {
                if (GetIsDynamic(grid.Columns[i]))
                    grid.Columns.RemoveAt(i);
            }

            for (int i = 0; i < steps.Count; i++)
            {
                var column = new DataGridTextColumn
                {
                    Width = 80,
                    Binding = new Binding($"[{i}]")
                    {
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    }
                };

                SetIsDynamic(column, true);
                grid.Columns.Add(column);
            }
        }
    }
}