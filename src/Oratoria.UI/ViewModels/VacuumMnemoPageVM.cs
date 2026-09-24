using Oratoria.Application.Algorithms;
using Oratoria.Application.Gateway1;
using Oratoria.Application.Gateway2;
using Oratoria.Application.Module2;
using Oratoria.Application.Module3;
using Oratoria.Application.Module4;
using Oratoria.Application.VacuumModule;
using Oratoria.Domain.Algorithms;
using Oratoria.Domain.Devices;
using Oratoria.Domain.Devices.Abstractions;
using Oratoria.Domain.Devices.AVRPump;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Devices.Statuses;
using Oratoria.UI.Controls.Controls.Mnemo;
using Oratoria.UI.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Oratoria.UI.ViewModels
{
    public class VacuumMnemoPageVM : INotifyPropertyChanged
    {
        private readonly VacuumContext _vacuumContext;
        private readonly Module2Context _module2;
        private readonly Module3Context _module3;
        private readonly Module4Context _module4;
        private readonly Gateway1Context _gateway1;
        private readonly Gateway2Context _gateway2;

        private readonly VacuumSystemPrepareAlgorithm _prepareAlgorithm;
        private readonly ToIdleAlgoritm _toIdleAlgoritm;

        public Action<object>? OpenDevice { get; set; }

        public string ModuleName => "Вакуумная система";

        public ICommand PrepareVacuumCommand
        {
            get
            {
                return new RelayCommand(async _ =>
                {
                    await _prepareAlgorithm.Start();
                    CommandManager.InvalidateRequerySuggested();
                },
                _ => _prepareAlgorithm.Status != AlgorithmStatus.Running && _prepareAlgorithm.CanStart());
            }
        }

        public ICommand StopPrepareVacuumCommand
        {
            get
            {
                return new RelayCommand(async _ =>
               {
                   _prepareAlgorithm.Cancel();
                   await _toIdleAlgoritm.ToIdle();
               },
               _ => _prepareAlgorithm.Status == AlgorithmStatus.Running);
            }
        }

        public string Module3_FK_KN_DU_63Name => _module3.FK_KN_DU_63.DeviceName;
        public StateColor Module3_FK_KN_DU_63State => MapStateToColor(_module3.FK_KN_DU_63);
        public ErrorStateIcon Module3_FK_KN_DU_63Error => MapErrorsToIcon(_module3.FK_KN_DU_63);
        public ICommand Module3_FK_KN_DU_63Command => Open(_module3.FK_KN_DU_63);

        public string FK_M3Name => _vacuumContext.FK_M3.DeviceName;
        public StateColor FK_M3State => MapStateToColor(_vacuumContext.FK_M3);
        public ErrorStateIcon FK_M3Error => MapErrorsToIcon(_vacuumContext.FK_M3);
        public ICommand FK_M3Command => Open(_vacuumContext.FK_M3);

        public string FK_M1Name => _vacuumContext.FK_M1.DeviceName;
        public StateColor FK_M1State => MapStateToColor(_vacuumContext.FK_M1);
        public ErrorStateIcon FK_M1Error => MapErrorsToIcon(_vacuumContext.FK_M1);
        public ICommand FK_M1Command => Open(_vacuumContext.FK_M1);

        public string FK_Shl1Name => _vacuumContext.FK_Shl1.DeviceName;
        public StateColor FK_Shl1State => MapStateToColor(_vacuumContext.FK_Shl1);
        public ErrorStateIcon FK_Shl1Error => MapErrorsToIcon(_vacuumContext.FK_Shl1);
        public ICommand FK_Shl1Command => Open(_vacuumContext.FK_Shl1);

        public string Module3_ShutterName => _module3.Shutter.DeviceName;
        public StateColor Module3_ShutterState => MapStateToColor(_module3.Shutter);
        public ErrorStateIcon Module3_ShutterError => MapErrorsToIcon(_module3.Shutter);
        public ICommand Module3_ShutterCommand => Open(_module3.Shutter);

        public string Shl1ShutterName => _gateway1.Shutter.DeviceName;
        public StateColor Shl1ShutterState => MapStateToColor(_gateway1.Shutter);
        public ErrorStateIcon Shl1ShutterError => MapErrorsToIcon(_gateway1.Shutter);
        public ICommand Shl1ShutterCommand => Open(_gateway1.Shutter);

        public string Module4_ShutterName => _module4.Shutter.DeviceName;
        public StateColor Module4_ShutterState => MapStateToColor(_module4.Shutter);
        public ErrorStateIcon Module4_ShutterError => MapErrorsToIcon(_module4.Shutter);
        public ICommand Module4_ShutterCommand => Open(_module4.Shutter);

        public string Module2_ShutterName => _module2.Shutter.DeviceName;
        public StateColor Module2_ShutterState => MapStateToColor(_module2.Shutter);
        public ErrorStateIcon Module2_ShutterError => MapErrorsToIcon(_module2.Shutter);
        public ICommand Module2_ShutterCommand => Open(_module2.Shutter);

        public string Shl2ShutterName => _gateway2.Shutter.DeviceName;
        public StateColor Shl2ShutterState => MapStateToColor(_gateway2.Shutter);
        public ErrorStateIcon Shl2ShutterError => MapErrorsToIcon(_gateway2.Shutter);
        public ICommand Shl2ShutterCommand => Open(_gateway2.Shutter);

        public string Module4_FK_KN_DU_63Name => _module4.FK_KN_DU_63.DeviceName;
        public StateColor Module4_FK_KN_DU_63State => MapStateToColor(_module4.FK_KN_DU_63);
        public ErrorStateIcon Module4_FK_KN_DU_63Error => MapErrorsToIcon(_module4.FK_KN_DU_63);
        public ICommand Module4_FK_KN_DU_63Command => Open(_module4.FK_KN_DU_63);

        public string FK_M4Name => _vacuumContext.FK_M4.DeviceName;
        public StateColor FK_M4State => MapStateToColor(_vacuumContext.FK_M4);
        public ErrorStateIcon FK_M4Error => MapErrorsToIcon(_vacuumContext.FK_M4);
        public ICommand FK_M4Command => Open(_vacuumContext.FK_M4);

        public string Module2_FK_KN_DU_63Name => _module2.FK_KN_DU_63.DeviceName;
        public StateColor Module2_FK_KN_DU_63State => MapStateToColor(_module2.FK_KN_DU_63);
        public ErrorStateIcon Module2_FK_KN_DU_63Error => MapErrorsToIcon(_module2.FK_KN_DU_63);
        public ICommand Module2_FK_KN_DU_63Command => Open(_module2.FK_KN_DU_63);

        public string FK_M2Name => _vacuumContext.FK_M2.DeviceName;
        public StateColor FK_M2State => MapStateToColor(_vacuumContext.FK_M2);
        public ErrorStateIcon FK_M2Error => MapErrorsToIcon(_vacuumContext.FK_M2);
        public ICommand FK_M2Command => Open(_vacuumContext.FK_M2);

        public string FK_Shl2Name => _vacuumContext.FK_Shl2.DeviceName;
        public StateColor FK_Shl2State => MapStateToColor(_vacuumContext.FK_Shl2);
        public ErrorStateIcon FK_Shl2Error => MapErrorsToIcon(_vacuumContext.FK_Shl2);
        public ICommand FK_Shl2Command => Open(_vacuumContext.FK_Shl2);

        public string KN1_TMName => _vacuumContext.KN1_TM.DeviceName;
        public StateColor KN1_TMState => MapStateToColor(_vacuumContext.KN1_TM);
        public ErrorStateIcon KN1_TMError => MapErrorsToIcon(_vacuumContext.KN1_TM);
        public ICommand KN1_TMCommand => Open(_vacuumContext.KN1_TM);

        public string KN_Zatvor_TMName => _vacuumContext.KN_Zatvor_TM.DeviceName;
        public StateColor KN_Zatvor_TMState => MapStateToColor(_vacuumContext.KN_Zatvor_TM);
        public ErrorStateIcon KN_Zatvor_TMError => MapErrorsToIcon(_vacuumContext.KN_Zatvor_TM);
        public ICommand KN_Zatvor_TMCommand => Open(_vacuumContext.KN_Zatvor_TM);

        public string FK_KN1Name => _vacuumContext.FK_KN1.DeviceName;
        public StateColor FK_KN1State => MapStateToColor(_vacuumContext.FK_KN1);
        public ErrorStateIcon FK_KN1Error => MapErrorsToIcon(_vacuumContext.FK_KN1);
        public ICommand FK_KN1Command => Open(_vacuumContext.FK_KN1);

        public string FK_TMName => _vacuumContext.FK_TM.DeviceName;
        public StateColor FK_TMState => MapStateToColor(_vacuumContext.FK_TM);
        public ErrorStateIcon FK_TMError => MapErrorsToIcon(_vacuumContext.FK_TM);
        public ICommand FK_TMCommand => Open(_vacuumContext.FK_TM);

        public string KN2_ShlName => _vacuumContext.KN2_Shl.DeviceName;
        public StateColor KN2_ShlState => MapStateToColor(_vacuumContext.KN2_Shl);
        public ErrorStateIcon KN2_ShlError => MapErrorsToIcon(_vacuumContext.KN2_Shl);
        public ICommand KN2_ShlCommand => Open(_vacuumContext.KN2_Shl);

        public string KN2_ZatvorName => _vacuumContext.KN2_Zatvor.DeviceName;
        public StateColor KN2_ZatvorState => MapStateToColor(_vacuumContext.KN2_Zatvor);
        public ErrorStateIcon KN2_ZatvorError => MapErrorsToIcon(_vacuumContext.KN2_Zatvor);
        public ICommand KN2_ZatvorCommand => Open(_vacuumContext.KN2_Zatvor);

        public string FK_OKName => _vacuumContext.FK_OK.DeviceName;
        public StateColor FK_OKState => MapStateToColor(_vacuumContext.FK_OK);
        public ErrorStateIcon FK_OKError => MapErrorsToIcon(_vacuumContext.FK_OK);
        public ICommand FK_OKCommand => Open(_vacuumContext.FK_OK);

        public string FK_APName => _vacuumContext.FK_AP.DeviceName;
        public StateColor FK_APState => MapStateToColor(_vacuumContext.FK_AP);
        public ErrorStateIcon FK_APError => MapErrorsToIcon(_vacuumContext.FK_AP);
        public ICommand FK_APCommand => Open(_vacuumContext.FK_AP);

        public string FK_AVRName => _vacuumContext.FK_AVR.DeviceName;
        public StateColor FK_AVRState => MapStateToColor(_vacuumContext.FK_AVR);
        public ErrorStateIcon FK_AVRError => MapErrorsToIcon(_vacuumContext.FK_AVR);
        public ICommand FK_AVRCommand => Open(_vacuumContext.FK_AVR);

        public string AP1Name => _vacuumContext.AP1.DeviceName;
        public StateColor AP1State => MapStateToColor(_vacuumContext.AP1);
        public ErrorStateIcon AP1Error => MapErrorsToIcon(_vacuumContext.AP1);
        public ICommand AP1Command => Open(_vacuumContext.AP1);

        public string AVRName => _vacuumContext.AVR.DeviceName;
        public StateColor AVRState => MapStateToColor(_vacuumContext.AVR);
        public ErrorStateIcon AVRError => MapErrorsToIcon(_vacuumContext.AVR);
        public ICommand AVRCommand => Open(_vacuumContext.AVR);

        public string FK_TrbName => _vacuumContext.FK_Trb.DeviceName;
        public StateColor FK_TrbState => MapStateToColor(_vacuumContext.FK_Trb);
        public ErrorStateIcon FK_TrbError => MapErrorsToIcon(_vacuumContext.FK_Trb);
        public ICommand FK_TrbCommand => Open(_vacuumContext.FK_Trb);

        public string Module3_TableName => _module3.Table.DeviceName;
        public StateColor Module3_TableState => MapStateToColor(_module3.Table);
        public ErrorStateIcon Module3_TableError => MapErrorsToIcon(_module3.Table);
        public ICommand Module3_TableCommand => Open(_module3.Table);
        public TableSlotPosition Module3_TablePosition => MapTablePosition(_module3.Table.Position);

        public string Module4_TableName => _module4.Table.DeviceName;
        public StateColor Module4_TableState => MapStateToColor(_module4.Table);
        public ErrorStateIcon Module4_TableError => MapErrorsToIcon(_module4.Table);
        public ICommand Module4_TableCommand => Open(_module4.Table);
        public TableSlotPosition Module4_TablePosition => MapTablePosition(_module4.Table.Position);

        public string Module2_TableName => _module2.Table.DeviceName;
        public StateColor Module2_TableState => MapStateToColor(_module2.Table);
        public ErrorStateIcon Module2_TableError => MapErrorsToIcon(_module2.Table);
        public ICommand Module2_TableCommand => Open(_module2.Table);
        public TableSlotPosition Module2_TablePosition => MapTablePosition(_module2.Table.Position);

        public string Door1Name => _gateway1.Door.DeviceName;
        public StateColor Door1State => MapStateToColor(_gateway1.Door);
        public ErrorStateIcon Door1Error => MapErrorsToIcon(_gateway1.Door);
        public ICommand Door1Command => Open(_gateway1.Door);

        public string Door2Name => _gateway2.Door.DeviceName;
        public StateColor Door2State => MapStateToColor(_gateway2.Door);
        public ErrorStateIcon Door2Error => MapErrorsToIcon(_gateway2.Door);
        public ICommand Door2Command => Open(_gateway2.Door);

        public string Module1LowVacuum => _vacuumContext.Module1LowPressure.CurrentPressureDisplay;
        public string Gateway1LowVacuum => _vacuumContext.Gateway1LowVacuum.CurrentPressureDisplay;
        public string Module3LowVacuum => _vacuumContext.Module3LowPressure.CurrentPressureDisplay;
        public string Module3HighVacuum => _module3.VICB.CurrentPressureDisplay;
        public string Gateway2LowVacuum => _vacuumContext.Gateway2LowVacuum.CurrentPressureDisplay;
        public string Module4LowVacuum => _vacuumContext.Module4LowPressure.CurrentPressureDisplay;
        public string Module4HighVacuum => _module4.VICB.CurrentPressureDisplay;
        public string Module2LowVacuum => _vacuumContext.Module2LowPressure.CurrentPressureDisplay;
        public string Module2HighVacuum => _module2.VICB.CurrentPressureDisplay;
        public string TransportLowVacuum => _vacuumContext.TransportLowVacuum.CurrentPressureDisplay;
        public string TransportHighVacuum => _vacuumContext.TransportHighVacuum.CurrentPressureDisplay;
        public string AVRLowVacuum => _vacuumContext.AVRLowVacuum.CurrentPressureDisplay;
        public string KNGatewayLowVacuum => _vacuumContext.KNGatewaytLowVacuum.CurrentPressureDisplay;
        public string KNGatewayHighVacuum => _vacuumContext.KNGatewayHighVacuum.CurrentPressureDisplay;
        public string KNTransportLowVacuum => _vacuumContext.KNTransportLowVacuum.CurrentPressureDisplay;
        public string KNTransportHighVacuum => _vacuumContext.KNTransportHighVacuum.CurrentPressureDisplay;

        public PipeColor ForPumpLine => MapPipe(_vacuumContext.AVR.State == PumpStatus.On);

        public PipeColor FkAvrLine => MapPipe(ForPumpLine == PipeColor.On && _vacuumContext.FK_AVR.State == OpenableStatus.Open);

        public PipeColor MainForLine => MapPipe((_vacuumContext.FK_OK.State == OpenableStatus.Open && ForPumpLine == PipeColor.On)
            || (_vacuumContext.FK_AP.State == OpenableStatus.Open && FkAvrLine == PipeColor.On));

        public PipeColor FkTRmLine => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_TM.State == OpenableStatus.Open);

        public PipeColor FkKN1Line => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_KN1.State == OpenableStatus.Open);

        public PipeColor KN1Line => MapPipe(FkKN1Line == PipeColor.On && _vacuumContext.KN1_TM.State == PumpStatus.On);

        public PipeColor ZatvorKN1Line => MapPipe(KN1Line == PipeColor.On && _vacuumContext.KN_Zatvor_TM.State == OpenableStatus.Open);

        public PipeColor FkTrbLine => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_Trb.State == OpenableStatus.Open
            || _vacuumContext.KN2_Shl.State == PumpStatus.On && _vacuumContext.KN2_Zatvor.State == OpenableStatus.Open);

        public PipeColor KN2Line => MapPipe(_vacuumContext.KN2_Shl.State == PumpStatus.On
            || (FkTrbLine == PipeColor.On && _vacuumContext.KN2_Zatvor.State == OpenableStatus.Open));

        public PipeColor Shl2Line => MapPipe(_vacuumContext.FK_Shl2.State == OpenableStatus.Open && FkTrbLine == PipeColor.On);

        public PipeColor Shl1Line => MapPipe(_vacuumContext.FK_Shl1.State == OpenableStatus.Open && FkTrbLine == PipeColor.On);

        public PipeColor FkM1Line => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_M1.State == OpenableStatus.Open);

        public PipeColor FkM2Line => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_M2.State == OpenableStatus.Open);

        public PipeColor FkM3Line => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_M3.State == OpenableStatus.Open);

        public PipeColor FkM4Line => MapPipe(MainForLine == PipeColor.On && _vacuumContext.FK_M4.State == OpenableStatus.Open);

        // Module 1 has no device context yet.
        public PipeColor FkKNM1Line => PipeColor.Off;

        public PipeColor FkKNM2Line => MapPipe(MainForLine == PipeColor.On && _module2.FK_KN_DU_63.State == OpenableStatus.Open);

        public PipeColor FkKNM3Line => MapPipe(MainForLine == PipeColor.On && _module3.FK_KN_DU_63.State == OpenableStatus.Open);

        public PipeColor FkKNM4Line => MapPipe(MainForLine == PipeColor.On && _module4.FK_KN_DU_63.State == OpenableStatus.Open);

        public VacuumMnemoPageVM(VacuumContext vacuum, Module2Context module2,
            Module3Context module3, Module4Context module4,
            Gateway1Context gateway1, Gateway2Context gateway2,
            VacuumSystemPrepareAlgorithm prepareAlgorithm,
            ToIdleAlgoritm toIdleAlgoritm)
        {
            _vacuumContext = vacuum;
            _module2 = module2;
            _module3 = module3;
            _module4 = module4;
            _gateway1 = gateway1;
            _gateway2 = gateway2;
            _prepareAlgorithm = prepareAlgorithm;
            _toIdleAlgoritm = toIdleAlgoritm;

            _module3.FK_KN_DU_63.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module3_FK_KN_DU_63State));
                OnPropertyChanged(nameof(FkKNM3Line));
            };
            _module3.FK_KN_DU_63.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module3_FK_KN_DU_63Error));
            _vacuumContext.FK_M3.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_M3State));
                OnPropertyChanged(nameof(FkM3Line));
            };
            _vacuumContext.FK_M3.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_M3Error));
            _vacuumContext.FK_M1.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_M1State));
                OnPropertyChanged(nameof(FkM1Line));
            };
            _vacuumContext.FK_M1.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_M1Error));
            _vacuumContext.FK_Shl1.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_Shl1State));
                OnPropertyChanged(nameof(Shl1Line));
            };
            _vacuumContext.FK_Shl1.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_Shl1Error));
            _module3.Shutter.StateChanged += () => OnPropertyChanged(nameof(Module3_ShutterState));
            _module3.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module3_ShutterError));
            _gateway1.Shutter.StateChanged += () => OnPropertyChanged(nameof(Shl1ShutterState));
            _gateway1.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Shl1ShutterError));
            _module4.Shutter.StateChanged += () => OnPropertyChanged(nameof(Module4_ShutterState));
            _module4.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module4_ShutterError));
            _module2.Shutter.StateChanged += () => OnPropertyChanged(nameof(Module2_ShutterState));
            _module2.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module2_ShutterError));
            _gateway2.Shutter.StateChanged += () => OnPropertyChanged(nameof(Shl2ShutterState));
            _gateway2.Shutter.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Shl2ShutterError));
            _module4.FK_KN_DU_63.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module4_FK_KN_DU_63State));
                OnPropertyChanged(nameof(FkKNM4Line));
            };
            _module4.FK_KN_DU_63.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module4_FK_KN_DU_63Error));
            _vacuumContext.FK_M4.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_M4State));
                OnPropertyChanged(nameof(FkM4Line));
            };
            _vacuumContext.FK_M4.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_M4Error));
            _module2.FK_KN_DU_63.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module2_FK_KN_DU_63State));
                OnPropertyChanged(nameof(FkKNM2Line));
            };
            _module2.FK_KN_DU_63.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module2_FK_KN_DU_63Error));
            _vacuumContext.FK_M2.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_M2State));
                OnPropertyChanged(nameof(FkM2Line));
            };
            _vacuumContext.FK_M2.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_M2Error));
            _vacuumContext.FK_Shl2.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_Shl2State));
                OnPropertyChanged(nameof(Shl2Line));
            };
            _vacuumContext.FK_Shl2.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_Shl2Error));
            _vacuumContext.KN1_TM.StateChanged += () =>
            {
                OnPropertyChanged(nameof(KN1_TMState));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
            };
            _vacuumContext.KN1_TM.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(KN1_TMError));
            _vacuumContext.KN_Zatvor_TM.StateChanged += () =>
            {
                OnPropertyChanged(nameof(KN_Zatvor_TMState));
                OnPropertyChanged(nameof(ZatvorKN1Line));
            };
            _vacuumContext.KN_Zatvor_TM.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(KN_Zatvor_TMError));
            _vacuumContext.FK_KN1.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_KN1State));
                OnPropertyChanged(nameof(FkKN1Line));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
            };
            _vacuumContext.FK_KN1.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_KN1Error));
            _vacuumContext.FK_TM.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_TMState));
                OnPropertyChanged(nameof(FkTRmLine));
            };
            _vacuumContext.FK_TM.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_TMError));
            _vacuumContext.KN2_Shl.StateChanged += () =>
            {
                OnPropertyChanged(nameof(KN2_ShlState));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
            };
            _vacuumContext.KN2_Shl.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(KN2_ShlError));
            _vacuumContext.KN2_Zatvor.StateChanged += () =>
            {
                OnPropertyChanged(nameof(KN2_ZatvorState));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
            };
            _vacuumContext.KN2_Zatvor.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(KN2_ZatvorError));
            _vacuumContext.FK_OK.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_OKState));
                OnPropertyChanged(nameof(MainForLine));
                OnPropertyChanged(nameof(FkTRmLine));
                OnPropertyChanged(nameof(FkKN1Line));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
                OnPropertyChanged(nameof(FkM1Line));
                OnPropertyChanged(nameof(FkM2Line));
                OnPropertyChanged(nameof(FkM3Line));
                OnPropertyChanged(nameof(FkM4Line));
                OnPropertyChanged(nameof(FkKNM2Line));
                OnPropertyChanged(nameof(FkKNM3Line));
                OnPropertyChanged(nameof(FkKNM4Line));
            };
            _vacuumContext.FK_OK.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_OKError));
            _vacuumContext.FK_AP.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_APState));
                OnPropertyChanged(nameof(MainForLine));
                OnPropertyChanged(nameof(FkTRmLine));
                OnPropertyChanged(nameof(FkKN1Line));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
                OnPropertyChanged(nameof(FkM1Line));
                OnPropertyChanged(nameof(FkM2Line));
                OnPropertyChanged(nameof(FkM3Line));
                OnPropertyChanged(nameof(FkM4Line));
                OnPropertyChanged(nameof(FkKNM2Line));
                OnPropertyChanged(nameof(FkKNM3Line));
                OnPropertyChanged(nameof(FkKNM4Line));
            };
            _vacuumContext.FK_AP.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_APError));
            _vacuumContext.FK_AVR.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_AVRState));
                OnPropertyChanged(nameof(FkAvrLine));
                OnPropertyChanged(nameof(MainForLine));
                OnPropertyChanged(nameof(FkTRmLine));
                OnPropertyChanged(nameof(FkKN1Line));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
                OnPropertyChanged(nameof(FkM1Line));
                OnPropertyChanged(nameof(FkM2Line));
                OnPropertyChanged(nameof(FkM3Line));
                OnPropertyChanged(nameof(FkM4Line));
                OnPropertyChanged(nameof(FkKNM2Line));
                OnPropertyChanged(nameof(FkKNM3Line));
                OnPropertyChanged(nameof(FkKNM4Line));
            };
            _vacuumContext.FK_AVR.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_AVRError));
            _vacuumContext.AP1.StateChanged += () => OnPropertyChanged(nameof(AP1State));
            _vacuumContext.AP1.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(AP1Error));
            _vacuumContext.AVR.StateChanged += () =>
            {
                OnPropertyChanged(nameof(AVRState));
                OnPropertyChanged(nameof(ForPumpLine));
                OnPropertyChanged(nameof(FkAvrLine));
                OnPropertyChanged(nameof(MainForLine));
                OnPropertyChanged(nameof(FkTRmLine));
                OnPropertyChanged(nameof(FkKN1Line));
                OnPropertyChanged(nameof(KN1Line));
                OnPropertyChanged(nameof(ZatvorKN1Line));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
                OnPropertyChanged(nameof(FkM1Line));
                OnPropertyChanged(nameof(FkM2Line));
                OnPropertyChanged(nameof(FkM3Line));
                OnPropertyChanged(nameof(FkM4Line));
                OnPropertyChanged(nameof(FkKNM2Line));
                OnPropertyChanged(nameof(FkKNM3Line));
                OnPropertyChanged(nameof(FkKNM4Line));
            };
            _vacuumContext.AVR.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(AVRError));
            _vacuumContext.FK_Trb.StateChanged += () =>
            {
                OnPropertyChanged(nameof(FK_TrbState));
                OnPropertyChanged(nameof(FkTrbLine));
                OnPropertyChanged(nameof(KN2Line));
                OnPropertyChanged(nameof(Shl2Line));
                OnPropertyChanged(nameof(Shl1Line));
            };
            _vacuumContext.FK_Trb.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(FK_TrbError));
            _module3.Table.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module3_TableState));
                OnPropertyChanged(nameof(Module3_TablePosition));
            };
            _module3.Table.PositionChanged += () => OnPropertyChanged(nameof(Module3_TablePosition));
            _module3.Table.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module3_TableError));
            _module4.Table.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module4_TableState));
                OnPropertyChanged(nameof(Module4_TablePosition));
            };
            _module4.Table.PositionChanged += () => OnPropertyChanged(nameof(Module4_TablePosition));
            _module4.Table.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module4_TableError));
            _module2.Table.StateChanged += () =>
            {
                OnPropertyChanged(nameof(Module2_TableState));
                OnPropertyChanged(nameof(Module2_TablePosition));
            };
            _module2.Table.PositionChanged += () => OnPropertyChanged(nameof(Module2_TablePosition));
            _module2.Table.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Module2_TableError));
            _gateway1.Door.StateChanged += () => OnPropertyChanged(nameof(Door1State));
            _gateway1.Door.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Door1Error));
            _gateway2.Door.StateChanged += () => OnPropertyChanged(nameof(Door2State));
            _gateway2.Door.DeviceErrors.ErrorChanged += _ => OnPropertyChanged(nameof(Door2Error));
            _vacuumContext.Module1LowPressure.StateChanged += () => OnPropertyChanged(nameof(Module1LowVacuum));
            _vacuumContext.Gateway1LowVacuum.StateChanged += () => OnPropertyChanged(nameof(Gateway1LowVacuum));
            _vacuumContext.Module3LowPressure.StateChanged += () => OnPropertyChanged(nameof(Module3LowVacuum));
            _module3.VICB.StateChanged += () => OnPropertyChanged(nameof(Module3HighVacuum));
            _vacuumContext.Gateway2LowVacuum.StateChanged += () => OnPropertyChanged(nameof(Gateway2LowVacuum));
            _vacuumContext.Module4LowPressure.StateChanged += () => OnPropertyChanged(nameof(Module4LowVacuum));
            _module4.VICB.StateChanged += () => OnPropertyChanged(nameof(Module4HighVacuum));
            _vacuumContext.Module2LowPressure.StateChanged += () => OnPropertyChanged(nameof(Module2LowVacuum));
            _module2.VICB.StateChanged += () => OnPropertyChanged(nameof(Module2HighVacuum));
            _vacuumContext.TransportLowVacuum.StateChanged += () => OnPropertyChanged(nameof(TransportLowVacuum));
            _vacuumContext.TransportHighVacuum.StateChanged += () => OnPropertyChanged(nameof(TransportHighVacuum));
            _vacuumContext.AVRLowVacuum.StateChanged += () => OnPropertyChanged(nameof(AVRLowVacuum));
            _vacuumContext.KNGatewaytLowVacuum.StateChanged += () => OnPropertyChanged(nameof(KNGatewayLowVacuum));
            _vacuumContext.KNGatewayHighVacuum.StateChanged += () => OnPropertyChanged(nameof(KNGatewayHighVacuum));
            _vacuumContext.KNTransportLowVacuum.StateChanged += () => OnPropertyChanged(nameof(KNTransportLowVacuum));
            _vacuumContext.KNTransportHighVacuum.StateChanged += () => OnPropertyChanged(nameof(KNTransportHighVacuum));
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

        private static StateColor MapStateToColor(CryogenicPump pump)
        {
            if (pump.State == PumpStatus.On)
                return StateColor.On;
            if (pump.State == PumpStatus.Off)
                return StateColor.Off;
            return StateColor.Transition;
        }

        private static StateColor MapStateToColor(AVRPump pump)
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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
