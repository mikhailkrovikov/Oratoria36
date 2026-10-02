using Oratoria.Persistence.DTOs;
using System.Collections.ObjectModel;

namespace Oratoria.UI.Services.Recipe
{
    public class ParameterCollection : ObservableCollection<Parameter>
    {
        private readonly ObservableCollection<RecipeStepDTO> _steps;

        public ParameterCollection(ObservableCollection<RecipeStepDTO> steps)
        {
            _steps = steps;
        }

        public void Add(string name, string interval)
        {
            Add(new Parameter(name, interval, _steps));
        }
    }
}
