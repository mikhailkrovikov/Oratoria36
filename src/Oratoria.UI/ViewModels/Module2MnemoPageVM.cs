using Oratoria.Application.Module2;
using Oratoria.Application.VacuumModule;
using Oratoria.Domain.Devices;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Devices.RRG;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.Infrastructure;
using ManipulatorPos = Oratoria.Domain.Devices.Statuses.ManipulatorPosition;
using ThrottlePos = Oratoria.Domain.Devices.Statuses.ThrottlePosition;
using Oratoria.UI.Controls.Controls.Mnemo;
using Oratoria.UI.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Oratoria.Application.TransportModule;

namespace Oratoria.UI.ViewModels
{
    public class Module2MnemoPageVM : INotifyPropertyChanged
    {
        private readonly Module2Context _context;
        private readonly VacuumContext _vacuumContext;
        private readonly TransportContext _transportContext;

        public Action<object>? OpenDevice { get; set; }

        public ModuleRealValueVM RealValues { get; }

        public object[] DeviceMenuSources => [_context, _vacuumContext.FK_M2];

        public string FK_KNName => _context.FK_KN_DU_63.DeviceName;
        public StateColor FK_KNState => MapStateToColor(_context.FK_KN_DU_63);
        public ErrorStateIcon FK_KNError => MapErrorsToIcon(_context.FK_KN_DU_63);
        public ICommand FK_KNCommand => Open(_context.FK_KN_DU_63);

        public string FK_M2Name => _vacuumContext.FK_M2.DeviceName;
        public StateColor FK_M2State => MapStateToColor(_vacuumContext.FK_M2);
        public ErrorStateIcon FK_M2Error => MapErrorsToIcon(_vacuumContext.FK_M2);
        public ICommand FK_M2Command => Open(_vacuumContext.FK_M2);

        public string NitrogenLeakerName => _context.NitrogenLeaker.DeviceName;
        public StateColor NitrogenLeakerState => MapStateToColor(_context.NitrogenLeaker);
        public ErrorStateIcon NitrogenLeakerError => MapErrorsToIcon(_context.NitrogenLeaker);
        public ICommand NitrogenLeakerCommand => Open(_context.NitrogenLeaker);

        public string ShutterName => _context.Shutter.DeviceName;
        public StateColor ShutterState => MapStateToColor(_context.Shutter);
        public ErrorStateIcon ShutterError => MapErrorsToIcon(_context.Shutter);
        public ICommand ShutterCommand => Open(_context.Shutter);

        public string FlapName => _context.Flap.DeviceName;
        public StateColor FlapState => MapStateToColor(_context.Flap);
        public ErrorStateIcon FlapError => MapErrorsToIcon(_context.Flap);
        public ICommand FlapCommand => Open(_context.Flap);

        public string HeaterName => _context.Heater.DeviceName;
        public StateColor HeaterState => MapStateToColor(_context.Heater);
        public ErrorStateIcon HeaterError => MapErrorsToIcon(_context.Heater);
        public ICommand HeaterCommand => Open(_context.Heater);

        public string Magnetron1Name => _context.Magnetron1.DeviceName;
        public StateColor Magnetron1State => MapStateToColor(_context.Magnetron1);
        public ErrorStateIcon Magnetron1Error => MapErrorsToIcon(_context.Magnetron1);
        public ICommand Magnetron1Command => Open(_context.Magnetron1);

        public string Magnetron2Name => _context.Magnetron2.DeviceName;
        public StateColor Magnetron2State => MapStateToColor(_context.Magnetron2);
        public ErrorStateIcon Magnetron2Error => MapErrorsToIcon(_context.Magnetron2);
        public ICommand Magnetron2Command => Open(_context.Magnetron2);

        public string Magnetron3Name => _context.Magnetron3.DeviceName;
        public StateColor Magnetron3State => MapStateToColor(_context.Magnetron3);
        public ErrorStateIcon Magnetron3Error => MapErrorsToIcon(_context.Magnetron3);
        public ICommand Magnetron3Command => Open(_context.Magnetron3);

        public string RRGLabel => _context.RRG.MaxFlowRate.Value.ToString();
        public StateColor RRGState => MapStateToColor(_context.RRG);
        public ErrorStateIcon RRGError => MapErrorsToIcon(_context.RRG);
        public double RRGSetPoint => _context.RRG.RRGSetPointValue;
        public double RRGRealValue => _context.RRG.RRGRealValue;
        public ICommand RRGCommand => Open(_context.RRG);

        public string CryoPumpName => _context.CryogenicPump.DeviceName;
        public StateColor CryoPumpState => MapStateToColor(_context.CryogenicPump);
        public ErrorStateIcon CryoPumpError => MapErrorsToIcon(_context.CryogenicPump);
        public ICommand CryoPumpCommand => Open(_context.CryogenicPump);

        public StateColor ManipulatorState => MapStateToColor(_context.Manipulator);
        public ManipulatorArmPosition ManipulatorPosition => MapManipulatorPosition(_context.Manipulator.Position);
        public string ManipulatorPositionLabel => _context.Manipulator.Position.GetDescription();
        public ErrorStateIcon ManipulatorError => MapErrorsToIcon(_context.Manipulator);
        public ICommand ManipulatorCommand => Open(_context.Manipulator);

        public string ThrottleName => _context.Throttle.DeviceName;
        public StateColor ThrottleState => MapStateToColor(_context.Throttle);
        public ThrottleFlapPosition ThrottlePosition => MapThrottlePosition(_context.Throttle.Position);
        public string ThrottlePositionLabel => _context.Throttle.Position.GetDescription();
        public ErrorStateIcon ThrottleError => MapErrorsToIcon(_context.Throttle);
        public ICommand ThrottleCommand => Open(_context.Throttle);

        public StateColor TableState => MapStateToColor(_context.Table);
        public TableSlotPosition TablePosition => MapTablePosition(_context.Table.Position);
        public ErrorStateIcon TableError => MapErrorsToIcon(_context.Table);
        public ICommand TableCommand => Open(_context.Table);



        public ICommand CarriageCommand => Open(_transportContext.Carriage);

        public string LowVacuumDisplay => _vacuumContext.Module2LowPressure.CurrentPressureDisplay;
        public string HighVacuumDisplay => _context.VICB.CurrentPressureDisplay;
        public string ModuleName => "Модуль 2 3-х позиционного напыления";

        public string ArgonLeakerName => _context.ArgonLeaker.DeviceName;
        public StateColor ArgonLeakerState => MapStateToColor(_context.ArgonLeaker);
        public double ArgonLeakerSetpoint => Math.Round((_context.ArgonLeaker.LeakerSetpoint?.Value ?? 0) * 10, 2);
        public ICommand ArgonLeakerCommand => Open(_context.ArgonLeaker);

        public PipeColor Line1 => MapPipe(_vacuumContext.FK_M2.State == OpenableStatus.Open);
        public PipeColor ArLine => PipeColor.On;
        public PipeColor N2Line => PipeColor.On;
        public PipeColor VacLine => PipeColor.On;
        public PipeColor Line2 => MapPipe(_context.RRG.State == RRGStatus.Open);
        public PipeColor Line3 => MapPipe(
            _context.ArgonLeaker.State == OpenableStatus.Open
            || (_context.RRG.State == RRGStatus.Open && _context.NitrogenLeaker.State == OpenableStatus.Open));
        public PipeColor Line4 => MapPipe(_context.FK_KN_DU_63.State == OpenableStatus.Open);
        public PipeColor Line5 => MapPipe(_context.CryogenicPump.State == PumpStatus.On);
        public PipeColor Line6 => MapPipe(Line5 == PipeColor.On && _context.Throttle.State == MechanicsPositions.Position1);

        public Module2MnemoPageVM(Module2Context context, VacuumContext vacuumContext, TransportContext transportContext)
        {
            _context = context;
            _vacuumContext = vacuumContext;
            _transportContext = transportContext;
            RealValues = new ModuleRealValueVM(_context);

            _context.FK_KN_DU_63.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_KNState));
                OnPropertyChanged(nameof(Line4));
            };
            _context.FK_KN_DU_63.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_KNError));
            _vacuumContext.FK_M2.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_M2State));
                OnPropertyChanged(nameof(Line1));
            };
            _vacuumContext.FK_M2.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_M2Error));
            _context.NitrogenLeaker.StateChanged += () =>
            {
                OnPropertyChanged(nameof(NitrogenLeakerState));
                OnPropertyChanged(nameof(Line3));
            };
            _context.NitrogenLeaker.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(NitrogenLeakerError));
            _context.Shutter.StateChanged += () => OnPropertyChanged(nameof(ShutterState));
            _context.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(ShutterError));
            _context.Flap.StateChanged += () => OnPropertyChanged(nameof(FlapState));
            _context.Flap.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FlapError));
            _context.Heater.StateChanged += () => OnPropertyChanged(nameof(HeaterState));
            _context.Heater.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(HeaterError));
            _context.Magnetron1.StateChanged += () => OnPropertyChanged(nameof(Magnetron1State));
            _context.Magnetron1.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Magnetron1Error));
            _context.Magnetron2.StateChanged += () => OnPropertyChanged(nameof(Magnetron2State));
            _context.Magnetron2.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Magnetron2Error));
            _context.Magnetron3.StateChanged += () => OnPropertyChanged(nameof(Magnetron3State));
            _context.Magnetron3.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Magnetron3Error));
            _context.RRG.StateChanged += () =>
            {
                OnPropertyChanged(nameof(RRGState));
                OnPropertyChanged(nameof(RRGSetPoint));
                OnPropertyChanged(nameof(RRGRealValue));
                OnPropertyChanged(nameof(Line2));
                OnPropertyChanged(nameof(Line3));
            };
            _context.RRG.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(RRGError));
            _context.RRG.MaxFlowRate.PropertyChanged += (_, __) => OnPropertyChanged(nameof(RRGLabel));
            _context.CryogenicPump.StateChanged += () =>
            {
                OnPropertyChanged(nameof(CryoPumpState));
                OnPropertyChanged(nameof(Line5));
                OnPropertyChanged(nameof(Line6));
            };
            _context.CryogenicPump.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(CryoPumpError));
            _context.Manipulator.StateChanged += () =>
            {
                OnPropertyChanged(nameof(ManipulatorState));
                OnPropertyChanged(nameof(ManipulatorPosition));
                OnPropertyChanged(nameof(ManipulatorPositionLabel));
            };
            _context.Manipulator.PositionChanged += () =>
            {
                OnPropertyChanged(nameof(ManipulatorPosition));
                OnPropertyChanged(nameof(ManipulatorPositionLabel));
            };
            _context.Manipulator.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(ManipulatorError));
            _context.Throttle.StateChanged += () =>
            {
                OnPropertyChanged(nameof(ThrottleState));
                OnPropertyChanged(nameof(ThrottlePosition));
                OnPropertyChanged(nameof(ThrottlePositionLabel));
                OnPropertyChanged(nameof(Line6));
            };
            _context.Throttle.PositionChanged += () =>
            {
                OnPropertyChanged(nameof(ThrottlePosition));
                OnPropertyChanged(nameof(ThrottlePositionLabel));
            };
            _context.Throttle.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(ThrottleError));
            _context.Table.StateChanged += () =>
            {
                OnPropertyChanged(nameof(TableState));
                OnPropertyChanged(nameof(TablePosition));
            };
            _context.Table.PositionChanged += () => OnPropertyChanged(nameof(TablePosition));
            _context.Table.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(TableError));
            _vacuumContext.Module2LowPressure.StateChanged += () => OnPropertyChanged(nameof(LowVacuumDisplay));
            _context.VICB.StateChanged += () => OnPropertyChanged(nameof(HighVacuumDisplay));
            _context.ArgonLeaker.StateChanged += () =>
            {
                OnPropertyChanged(nameof(ArgonLeakerState));
                OnPropertyChanged(nameof(ArgonLeakerSetpoint));
                OnPropertyChanged(nameof(Line3));
            };

            _context.ArgonLeaker.LeakerSetpoint?.OnSignalChanged += _ => OnPropertyChanged(nameof(ArgonLeakerSetpoint));
        }

        private static StateColor MapStateToColor(OpenableDevice device)
        {
            if (device.State == OpenableStatus.Open)
                return StateColor.On;
            if (device.State == OpenableStatus.Close)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static StateColor MapStateToColor(PowerDevice device)
        {
            if (device.State == PowerDeviceStatus.On)
                return StateColor.On;
            if (device.State == PowerDeviceStatus.Off)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static StateColor MapStateToColor(RRG rrg)
        {
            if (rrg.State == RRGStatus.Open)
                return StateColor.On;
            if (rrg.State == RRGStatus.Close)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static StateColor MapStateToColor(CryogenicPump pump)
        {
            if (pump.State == PumpStatus.On)
                return StateColor.On;
            if (pump.State == PumpStatus.Off)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static StateColor MapStateToColor<TPos, TErr>(MechanicDevice<TPos, TErr> device)
            where TPos : Enum
            where TErr : Enum
        {
            return device.State switch
            {
                MechanicsPositions.Position1 or
                MechanicsPositions.Position2 or
                MechanicsPositions.Position3 => StateColor.On,
                MechanicsPositions.Uncertain => StateColor.Uncertain,
                MechanicsPositions.Indefinite => StateColor.Indefinite,
                _ => StateColor.Transition
            };
        }

        private static ManipulatorArmPosition MapManipulatorPosition(ManipulatorPos position) => position switch
        {
            ManipulatorPos.Module => ManipulatorArmPosition.Module,
            ManipulatorPos.Home => ManipulatorArmPosition.Home,
            ManipulatorPos.Transport => ManipulatorArmPosition.Transport,
            ManipulatorPos.Uncertain => ManipulatorArmPosition.Uncertain,
            ManipulatorPos.Transition => ManipulatorArmPosition.Transition,
            _ => ManipulatorArmPosition.Indefinite
        };

        private static ThrottleFlapPosition MapThrottlePosition(ThrottlePos position) => position switch
        {
            ThrottlePos.Open => ThrottleFlapPosition.Open,
            ThrottlePos.Close => ThrottleFlapPosition.Close,
            ThrottlePos.Throttling => ThrottleFlapPosition.Throttling,
            ThrottlePos.Uncertain => ThrottleFlapPosition.Uncertain,
            ThrottlePos.Transition => ThrottleFlapPosition.Transition,
            _ => ThrottleFlapPosition.Indefinite
        };

        private static TableSlotPosition MapTablePosition(ModuleTablePosition position) => position switch
        {
            ModuleTablePosition.Home => TableSlotPosition.Home,
            ModuleTablePosition.Rollback => TableSlotPosition.Rollback,
            ModuleTablePosition.Processing => TableSlotPosition.Processing,
            ModuleTablePosition.Uncertain => TableSlotPosition.Uncertain,
            ModuleTablePosition.Transition => TableSlotPosition.Transition,
            _ => TableSlotPosition.Indefinite
        };

        private ICommand Open(object device) => new RelayCommand(_ => OpenDevice?.Invoke(device));

        private static PipeColor MapPipe(bool isOn) => isOn ? PipeColor.On : PipeColor.Off;

        private static ErrorStateIcon MapErrorsToIcon<TStatus, TError>(IDevice<TStatus, TError> device)
            where TStatus : Enum
            where TError : Enum
        {
            return device.DeviceErrors.GetHighestCategory() switch
            {
                DeviceErrorCategory.Error or DeviceErrorCategory.Fatal => ErrorStateIcon.Error,
                DeviceErrorCategory.Warn => ErrorStateIcon.Warning,
                _ => ErrorStateIcon.None
            };
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
