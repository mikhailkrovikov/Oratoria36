using Oratoria.Persistence.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Oratoria.UI.Services.Recipe
{
    public class Parameter : INotifyPropertyChanged
    {
        private readonly ObservableCollection<RecipeStepDTO> _steps;

        public string Name { get; }
        public string Interval { get; }

        public Parameter(string name, string interval, ObservableCollection<RecipeStepDTO> steps)
        {
            Name = name;
            Interval = interval;
            _steps = steps;
        }

        public string this[int stepIndex]
        {
            get
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return string.Empty;

                return _steps[stepIndex].Parameters.TryGetValue(Name, out var value)
                    ? value.ToString(CultureInfo.CurrentCulture)
                    : string.Empty;
            }
            set
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return;

                if (string.IsNullOrWhiteSpace(value))
                    _steps[stepIndex].Parameters.Remove(Name);
                else
                    _steps[stepIndex].Parameters[Name] =
                        double.Parse(value, NumberStyles.Float, CultureInfo.CurrentCulture);

                OnPropertyChanged("Item[]");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
