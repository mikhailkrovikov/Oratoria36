using System.ComponentModel;
using System.Runtime.CompilerServices;
using Oratoria.Application.Module2;
using Oratoria.Domain.Devices.Magnetron;

namespace Oratoria.UI.ViewModels;

public class ModuleRealValueVM : INotifyPropertyChanged
{
    private readonly Module2Context _context;

    public string ModuleName => "Модуль 2 3-х позиционного напыления";

    public double HeaterPower => _context.Heater.CalculatedPower;
    public double HeaterPowerSetPoint => _context.Heater.HeaterPowerSetPoint.Value;

    public double Temperature => _context.Heater.CalcTemperature;
    public double HeaterTemperatureSetpoint => 0;

    public string Pressure => _context.VICB.CurrentPressureDisplay;
    public double PressureSetpoint => 0;

    public double RRGRealValue => _context.RRG.RRGRealValue;
    public double RRGSetPointValue => _context.RRG.RRGSetPointValue;

    public double Magnetron1Power => _context.Magnetron1.CalculatedPower;
    public double Magnetron1PowerSetpoint => _context.Magnetron1.MagnetronPowerSetPoint.Value;

    public double Magnetron2Power => _context.Magnetron2.CalculatedPower;
    public double Magnetron2PowerSetpoint => _context.Magnetron2.MagnetronPowerSetPoint.Value;

    public double Magnetron3Power => _context.Magnetron3.CalculatedPower;
    public double Magnetron3PowerSetpoint => _context.Magnetron3.MagnetronPowerSetPoint.Value;

    public double Resource1Power => 0;
    public double Resource2Power => 0;
    public double Resource3Power => 0;

    public ModuleRealValueVM(Module2Context context)
    {
        _context = context;

        _context.Heater.HeaterCurrent.OnSignalChanged += _ => OnPropertyChanged(nameof(HeaterPower));
        _context.Heater.HeaterVoltage.OnSignalChanged += _ => OnPropertyChanged(nameof(HeaterPower));
        _context.Heater.HeaterTemp.OnSignalChanged += _ => OnPropertyChanged(nameof(Temperature));
        _context.Heater.HeaterPowerSetPoint.OnSignalChanged += _ => OnPropertyChanged(nameof(HeaterPowerSetPoint));

        SubscribeMagnetron(_context.Magnetron1, nameof(Magnetron1Power), nameof(Magnetron1PowerSetpoint));
        SubscribeMagnetron(_context.Magnetron2, nameof(Magnetron2Power), nameof(Magnetron2PowerSetpoint));
        SubscribeMagnetron(_context.Magnetron3, nameof(Magnetron3Power), nameof(Magnetron3PowerSetpoint));

        _context.RRG.StateChanged += () =>
        {
            OnPropertyChanged(nameof(RRGRealValue));
            OnPropertyChanged(nameof(RRGSetPointValue));
        };

        _context.VICB.StateChanged += () => OnPropertyChanged(nameof(Pressure));
    }

    private void SubscribeMagnetron(Magnetron magnetron, string powerProperty, string setpointProperty)
    {
        magnetron.MagnetronCurrent.OnSignalChanged += _ => OnPropertyChanged(powerProperty);
        magnetron.MagnetronVoltage.OnSignalChanged += _ => OnPropertyChanged(powerProperty);
        magnetron.MagnetronPowerSetPoint.OnSignalChanged += _ => OnPropertyChanged(setpointProperty);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
