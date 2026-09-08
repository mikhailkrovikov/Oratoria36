using Oratoria.Persistence.Services;
using System.Collections.ObjectModel;

namespace Oratoria.Application.Recipe
{
    public abstract class Recipe<TModule, TStage>
    {
        public string Name { get; set; }

        public readonly TModule ModuleContext;

        public ObservableCollection<TStage> Stages { get; }

        private readonly RecipeService _recipeService;

        public Recipe(string name, TModule moduleContext)
        {
            Name = name;
            ModuleContext = moduleContext;
            Stages = new();
        }
    }
}
