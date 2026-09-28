using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Oratoria.Domain.Devices;
using Oratoria.UI.Services;

namespace Oratoria.UI.ViewModels;

public class DeviceActionVM
{
    private readonly object _device;
    private readonly MethodInfo _method;
    private readonly Func<bool> _isRunning;
    private readonly Action<bool> _setRunning;
    private readonly List<ActionParameterBinding> _parameters = new();

    public string DisplayName { get; }
    public ICommand Command { get; }
    public FrameworkElement? ParameterControl { get; }

    public DeviceActionVM(
        object device,
        MethodInfo method,
        DeviceActionAttribute action,
        Func<bool> isRunning,
        Action<bool> setRunning)
    {
        _device = device;
        _method = method;
        _isRunning = isRunning;
        _setRunning = setRunning;
        DisplayName = action.Name;
        ParameterControl = CreateParameterControl(method.GetParameters());
        Command = new RelayCommand(
            async _ =>
            {
                _setRunning(true);
                try
                {
                    var args = GetArgs();
                    var result = _method.Invoke(_device, args);
                    if (result is Task task)
                        await task;
                }
                finally
                {
                    _setRunning(false);
                }
            },
            _ => !_isRunning() && CanExecute());
    }

    public static bool CanBuild(MethodInfo method)
    {
        foreach (var parameter in method.GetParameters())
        {
            if (!ActionParameterBinding.IsSupported(parameter.ParameterType))
                return false;
        }

        return true;
    }

    private FrameworkElement? CreateParameterControl(ParameterInfo[] parameters)
    {
        if (parameters.Length == 0)
            return null;

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center
        };

        foreach (var parameter in parameters)
        {
            var binding = new ActionParameterBinding(parameter);
            _parameters.Add(binding);

            if (binding.Control != null)
                panel.Children.Add(binding.Control);
        }

        return panel.Children.Count == 0 ? null : panel;
    }

    private object?[] GetArgs() => _parameters.Select(p => p.GetValue()).ToArray();

    private bool CanExecute()
    {
        var canMethod = _device.GetType().GetMethod("Can" + _method.Name, BindingFlags.Public | BindingFlags.Instance);
        if (canMethod == null || canMethod.GetParameters().Length != 0 || canMethod.ReturnType != typeof(bool))
            return true;

        return (bool)canMethod.Invoke(_device, null)!;
    }
}
