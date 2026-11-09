using Oratoria.Persistence;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Oratoria.UI.Services.Recipe
{
    public class Parameter : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Stage> _steps;
        private readonly Func<Stage, double?> _getValue;
        private readonly Action<Stage, double?> _setValue;

        public string Name { get; }
        public string Interval { get; }

        public Parameter(string name, string interval, ObservableCollection<Stage> steps,
            Func<Stage, double?> getValue, Action<Stage, double?> setValue)
        {
            Name = name;
            Interval = interval;
            _steps = steps;
            _getValue = getValue;
            _setValue = setValue;
        }

        public string this[int stepIndex]
        {
            get
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return string.Empty;

                return _getValue(_steps[stepIndex])?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
            }
            set
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return;

                double? number = string.IsNullOrWhiteSpace(value)
                    ? null
                    : double.Parse(value, NumberStyles.Float, CultureInfo.CurrentCulture);

                _setValue(_steps[stepIndex], number);
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
