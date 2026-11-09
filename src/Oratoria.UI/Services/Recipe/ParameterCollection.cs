using Oratoria.Persistence;
using System.Collections.ObjectModel;

namespace Oratoria.UI.Services.Recipe
{
    public class ParameterCollection : ObservableCollection<Parameter>
    {
        private readonly ObservableCollection<Stage> _steps;

        public ParameterCollection(ObservableCollection<Stage> steps)
        {
            _steps = steps;
        }

        public void Add(string name, string interval,
            Func<Stage, double?> getValue, Action<Stage, double?> setValue)
        {
            Add(new Parameter(name, interval, _steps, getValue, setValue));
        }
    }
}
