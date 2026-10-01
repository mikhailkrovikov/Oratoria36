using Oratoria.Application.Module2;
using Oratoria.Application.Recipe;
using Oratoria.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Oratoria.UI.Services.Recipe;

namespace Oratoria.UI.ViewModels
{
    public class Module2RecipePageVM : INotifyPropertyChanged
    {
        private readonly Module2Context _context;

        public ObservableCollection<Recipe> Recipes { get; } = new();
        public ObservableCollection<Step> Steps { get; } = new();
        public ObservableCollection<ParameterRow> Parameters { get; } = new();

        public RelayCommand AddStepCommand { get; }
        public RelayCommand RemoveStepCommand;
        public RelayCommand ClearRecipeCommand;
        public RelayCommand RemoveRecipeCommand;
        public RelayCommand LoadRecipeCommand;
        public RelayCommand DeleteRecipeCommand;

        public Module2RecipePageVM(Module2Context context)
        {
            _context = context;
            AddStepCommand = new RelayCommand(_ => AddStep());
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            var defs = new (string Name, string Interval)[]
            {
                ("Время нагрева, сек",        "-"),
                ("Мощность нагрева, Вт",      "4000"),
                ("Давление, Па",              "0,13 - 1,33"),
                ("Расход, л/ч",               "-"),
                ("Время напыления, сек",      "-"),
                ("Время отпыла, сек",         "-"),
                ("Мощность магнетрона 1, Вт", "4000"),
                ("Мощность магнетрона 2, Вт", "4000"),
                ("Мощность магнетрона 3, Вт", "4000"),
            };

            foreach (var (name, interval) in defs)
            {
                var row = new Parameter
                {
                    Name = name,
                    Interval = interval,
                    Value = string.Empty
                };

                Parameters.Add(new ParameterRow(row, Steps));
            }

            AddStep();
        }

        private void AddStep()
        {
            var step = new Step { Number = Steps.Count + 1 };

            foreach (var row in Parameters)
            {
                step.Parameters.Add(new Parameter
                {
                    Name = row.Row.Name,
                    Interval = row.Row.Interval,
                    Value = string.Empty
                });
            }

            Steps.Add(step);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public sealed class ParameterRow : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Step> _steps;

        public Parameter Row { get; }

        public string Name => Row.Name;
        public string Interval => Row.Interval;

        public ParameterRow(Parameter row, ObservableCollection<Step> steps)
        {
            Row = row;
            _steps = steps;
        }
        public string this[int stepIndex]
        {
            get
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return string.Empty;

                var p = _steps[stepIndex].Parameters.Find(x => x.Name == Row.Name);
                return p?.Value ?? string.Empty;
            }
            set
            {
                if (stepIndex < 0 || stepIndex >= _steps.Count) return;

                var p = _steps[stepIndex].Parameters.Find(x => x.Name == Row.Name);
                if (p == null)
                {
                    p = new Parameter
                    {
                        Name = Row.Name,
                        Interval = Row.Interval,
                        Value = value
                    };
                    _steps[stepIndex].Parameters.Add(p);
                }
                else
                {
                    p.Value = value;
                }

                OnPropertyChanged($"Item[{stepIndex}]");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}