using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oratoria.UI.ViewModels;
using Oratoria.UI.Controls.DialogWindows;

namespace Oratoria.UI.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Module2RecipePage.xaml
    /// </summary>
    public partial class Module2RecipePage : Page
    {
        public Module2RecipePage(Module2RecipePageVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await ((Module2RecipePageVM)DataContext).LoadRecipes();
            }
            catch (Exception ex)
            {
                UserMessageBox.Show(ex.Message, "Загрузка рецептов", MBType.Error);
            }
        }
    }
}
