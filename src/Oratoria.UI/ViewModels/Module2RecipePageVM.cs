using Recipe = Oratoria.Persistence.Recipe;
using Oratoria.Application.Module2;
using Oratoria.Persistence.Services;
using Oratoria.UI.Controls.DialogWindows;
using Oratoria.UI.Services;
using Oratoria.UI.Services.Recipe;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Oratoria.Persistence;

namespace Oratoria.UI.ViewModels
{
    public class Module2RecipePageVM : INotifyPropertyChanged
    {
        private readonly Module2Context _context;
        private readonly IRecipeService _recipeService;

        private string _recipeName = "Введите название...";
        private Recipe? _selectedRecipe;
        private Stage? _selectedStep;

        public ObservableCollection<Recipe> Recipes { get; } = new();
        public ObservableCollection<Stage> Steps { get; } = new();
        public ParameterCollection Parameters { get; }

        public string RecipeName
        {
            get => _recipeName;
            set
            {
                _recipeName = value;
                OnPropertyChanged();
            }
        }

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public Stage? SelectedStep
        {
            get => _selectedStep;
            set
            {
                _selectedStep = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddStepCommand
        {
            get => new RelayCommand((_) =>
            {
                var step = new Stage { Number = Steps.Count + 1 };
                Steps.Add(step);
                SelectedStep = step;
            });
        }

        public ICommand RemoveStepCommand
        {
            get => new RelayCommand((_) =>
            {
                if (SelectedStep == null) return;

                Steps.Remove(SelectedStep);
                var steps = Steps.ToList();
                Steps.Clear();
                foreach (var step in steps)
                {
                    step.Number = Steps.Count + 1;
                    Steps.Add(step);
                }
                SelectedStep = Steps.LastOrDefault();
                RefreshParameters();
            });
        }

        public ICommand ClearRecipeCommand
        {
            get => new RelayCommand((_) =>
            {
                Steps.Clear();
                SelectedStep = null;
                RefreshParameters();
            });
        }

        public ICommand LoadRecipeCommand
        {
            get => new RelayCommand(async (_) =>
            {
                if (SelectedRecipe?.Id == null) return;

                try
                {
                    var recipe = await _recipeService.ReadRecipe(SelectedRecipe.Id.Value);
                    if (recipe == null) return;

                    RecipeName = recipe.Name;
                    Steps.Clear();
                    foreach (var step in recipe.Stages)
                        Steps.Add(step);

                    SelectedStep = Steps.LastOrDefault();
                    RefreshParameters();
                }
                catch (Exception ex)
                {
                    UserMessageBox.Show(ex.Message, "Загрузка рецепта", MBType.Error);
                }
            });
        }

        public ICommand DeleteRecipeCommand
        {
            get => new RelayCommand(async (_) =>
            {
                if (SelectedRecipe?.Id == null) return;

                try
                {
                    await _recipeService.DeleteRecipe(SelectedRecipe.Id.Value);
                    await LoadRecipes();
                }
                catch (Exception ex)
                {
                    UserMessageBox.Show(ex.Message, "Удаление рецепта", MBType.Error);
                }
            });
        }

        public ICommand SaveRecipeCommand
        {
            get => new RelayCommand(async (_) =>
            {
                if (string.IsNullOrWhiteSpace(RecipeName))
                {
                    UserMessageBox.Show("Введите название рецепта.", "Сохранение рецепта");
                    return;
                }

                try
                {
                    var name = RecipeName.Trim();
                    var existing = await _recipeService.ReadRecipe(name);
                    if (existing != null)
                    {
                        var result = UserMessageBox.Show(
                            "Перезаписать рецепт с этим именем?",
                            "Сохранение рецепта", MBType.Warning, MBButtons.Okcancel);

                        if (result != MBResult.Ok) return;
                    }

                    var recipe = new Recipe
                    {
                        Id = existing?.Id,
                        Name = name,
                        ModuleId = 2,
                        Stages = Steps.Select(stage => stage.Copy()).ToList()
                    };

                    var saved = existing == null
                        ? await _recipeService.CreateRecipe(recipe)
                        : await _recipeService.Update(recipe);

                    if (!saved)
                    {
                        UserMessageBox.Show("Не удалось сохранить рецепт.", "Сохранение рецепта", MBType.Error);
                        return;
                    }

                    RecipeName = name;
                    await LoadRecipes();
                    SelectedRecipe = Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                }
                catch (Exception ex)
                {
                    UserMessageBox.Show(ex.Message, "Сохранение рецепта", MBType.Error);
                }
            });
        }

        public Module2RecipePageVM(Module2Context context, IRecipeService recipeService)
        {
            _context = context;
            _recipeService = recipeService;
            Parameters = new ParameterCollection(Steps);
            Parameters.Add("Время нагрева, сек", "-", stage => stage.HeatingTime, (stage, value) => stage.HeatingTime = value);
            Parameters.Add("Мощность нагрева, Вт", "4000", stage => stage.HeatingPower, (stage, value) => stage.HeatingPower = value);
            Parameters.Add("Давление, Па", "0,13 - 1,33", stage => stage.Pressure, (stage, value) => stage.Pressure = value);
            Parameters.Add("Расход, л/ч", $"{_context.RRG.MaxFlowRate.Value}", stage => stage.Consumption, (stage, value) => stage.Consumption = value);
            Parameters.Add("Время напыления, сек", "-", stage => stage.SputteringTime, (stage, value) => stage.SputteringTime = value);
            Parameters.Add("Время отпыла, сек", "-", stage => stage.PreSputteringTime, (stage, value) => stage.PreSputteringTime = value);
            Parameters.Add("Мощность магнетрона 1, Вт", "4000", stage => stage.Magn1Power, (stage, value) => stage.Magn1Power = value);
            Parameters.Add("Мощность магнетрона 2, Вт", "4000", stage => stage.Magn2Power, (stage, value) => stage.Magn2Power = value);
            Parameters.Add("Мощность магнетрона 3, Вт", "4000", stage => stage.Magn3Power, (stage, value) => stage.Magn3Power = value);

            var step = new Stage { Number = 1 };
            Steps.Add(step);
            SelectedStep = step;
        }

        public async Task LoadRecipes()
        {
            var recipes = await _recipeService.ReadRecipes(2);
            SelectedRecipe = null;
            Recipes.Clear();
            foreach (var recipe in recipes)
                Recipes.Add(recipe);
        }

        private void RefreshParameters()
        {
            foreach (var parameter in Parameters)
                parameter.OnPropertyChanged("Item[]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
