using Oratoria.Application.VacuumModule.Signals;
using Oratoria.Application.Module4.Signals;
using Oratoria.Application.Module3.Signals;
using Oratoria.Application.Module2.Signals;
using DigitalTwin;
using Oratoria.Application.Module2;
using Oratoria.Application.Module3;
using Oratoria.Application.Module4;
using Oratoria.Application.VacuumModule;

namespace Oratoria.Application
{
    public class DTInitializer
    {
        public DTInitializer(
            Module2Context module2context,
            Module3Context module3Context,
            Module4Context module4Context,
            VacuumContext vacuumContext,
            IRegister model)
        {
            InitTechnologyModule(module2context, model.GetModule(nameof(Module2Signals)));
            InitTechnologyModule(module3Context, model.GetModule(nameof(Module3Signals)));
            InitTechnologyModule(module4Context, model.GetModule(nameof(Module4Signals)));
            InitVacuumModule(vacuumContext, model.GetModule(nameof(VacuumSignals)));
        }

        public static void InitTechnologyModule(TechnologyModuleContext context, IRegister model)
        {
            model.SetDoubleInput(context.VICB.PressureSignal.PinNumber, 10.0);

            var manOuts = new ushort[]
            {
                context.Manipulator.Position1Out.PinNumber,
                context.Manipulator.Position2Out.PinNumber,
                context.Manipulator.Position3Out.PinNumber,
                context.Manipulator.TormosOut.PinNumber,
                context.Manipulator.ReversOut.PinNumber,
                context.Manipulator.Actuator.PinNumber
            };

            var manIns = new ushort[]
            {
                context.Manipulator.Position1In.PinNumber,
                context.Manipulator.Position2In.PinNumber,
                context.Manipulator.Position3In.PinNumber,
                context.Manipulator.TormosIn.PinNumber,
                context.Manipulator.ReversIn.PinNumber
            };

            var tabOuts = new ushort[]
            {
                context.Table.Position1Out.PinNumber,
                context.Table.Position2Out.PinNumber,
                context.Table.Position3Out.PinNumber,
                context.Table.TormosOut.PinNumber,
                context.Table.ReversOut.PinNumber,
                context.Table.Actuator.PinNumber
            };

            var tabIns = new ushort[]
            {
                context.Table.Position1In.PinNumber,
                context.Table.Position2In.PinNumber,
                context.Table.Position3In.PinNumber,
                context.Table.TormosIn.PinNumber,
                context.Table.ReversIn.PinNumber
            };


            var tvOuts = new ushort[]
            {
                context.Throttle.Position1Out.PinNumber,
                context.Throttle.Position2Out.PinNumber,
                context.Throttle.Position3Out.PinNumber,
                context.Throttle.TormosOut.PinNumber,
                context.Throttle.ReversOut.PinNumber,
                context.Throttle.Actuator.PinNumber
            };

            var tvIns = new ushort[]
            {
                context.Throttle.Position1In.PinNumber,
                context.Throttle.Position2In.PinNumber,
                context.Throttle.Position3In.PinNumber,
                context.Throttle.TormosIn.PinNumber,
                context.Throttle.ReversIn.PinNumber
            };

            model.RegisterDevice<bool>(context.FK_KN_DU_63.OpenSignal.PinNumber, context.FK_KN_DU_63.IsOpenSignal.PinNumber, context.FK_KN_DU_63.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.Shutter.OpenSignal.PinNumber, context.Shutter.IsOpenSignal.PinNumber, context.Shutter.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.Flap.CloseSignal.PinNumber, context.Flap.IsCloseSignal.PinNumber, context.Flap.IsOpenSignal.PinNumber, 300);

            model.RegisterDevice<bool>(context.NitrogenLeaker.OpenSignal.PinNumber, context.NitrogenLeaker.IsOpenSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.ArgonLeaker.OpenSignal.PinNumber, context.ArgonLeaker.IsOpenSignal.PinNumber, 300);

            model.RegisterDevice<double>(context.RRG.RRGSetpointSignal.PinNumber, context.RRG.RRGRealValueSignal.PinNumber, 1000);

            model.RegisterDevice<bool>(context.Heater.PowerOn.PinNumber, context.Heater.IsPowerOn.PinNumber, 600);
            model.RegisterDevice<double>(context.Heater.HeaterPowerSetPoint.PinNumber, context.Heater.HeaterVoltage.PinNumber, 600);

            model.RegisterDevice<bool>(context.Magnetron1.RotationOn.PinNumber, context.Magnetron1.IsRotating.PinNumber, 600);
            model.RegisterDevice<bool>(context.Magnetron1.PowerOn.PinNumber, context.Magnetron1.IsPowerOn.PinNumber, 600);
            model.RegisterDevice<double>(context.Magnetron1.MagnetronPowerSetPoint.PinNumber, context.Magnetron1.MagnetronVoltage.PinNumber, 600);
            model.RegisterDevice<bool>(context.Magnetron2.PowerOn.PinNumber, context.Magnetron2.IsPowerOn.PinNumber, 600);
            model.RegisterDevice<double>(context.Magnetron2.MagnetronPowerSetPoint.PinNumber, context.Magnetron2.MagnetronVoltage.PinNumber, 600);
            model.RegisterDevice<bool>(context.Magnetron3.PowerOn.PinNumber, context.Magnetron3.IsPowerOn.PinNumber, 600);
            model.RegisterDevice<double>(context.Magnetron3.MagnetronPowerSetPoint.PinNumber, context.Magnetron3.MagnetronVoltage.PinNumber, 600);

            model.RegisterMechanicDevice<bool>(manOuts, manIns, 500);
            model.RegisterMechanicDevice<bool>(tabOuts, tabIns, 500);
            model.RegisterMechanicDevice<bool>(tvOuts, tvIns, 500);
        }

        public static void InitVacuumModule(VacuumContext context, IRegister model)
        {
            // 10 В: 100 000 Па для НВ и верхний предел 1333 Па для ВВ.
            model.SetDoubleInput(context.Module1LowPressure.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.Module2LowPressure.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.Module3LowPressure.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.Module4LowPressure.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.TransportLowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.TransportHighVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.Gateway1LowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.Gateway2LowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.KNTransportLowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.KNTransportHighVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.KNGatewaytLowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.KNGatewayHighVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.TrupoprovodLowVacuum.PressureSignal.PinNumber, 10.0);
            model.SetDoubleInput(context.AVRLowVacuum.PressureSignal.PinNumber, 10.0);

            model.RegisterDevice<bool>(context.AVR.OilPumpOn.PinNumber, context.AVR.IsOilPumpOn.PinNumber, 500);
            model.RegisterDevice<bool>(context.AVR.RutsPumpOn.PinNumber, context.AVR.IsRutsPumpOn.PinNumber, 500);

            model.RegisterDevice<bool>(context.AP1.PowerOn.PinNumber, context.AP1.IsPowerOn.PinNumber, 300);

            var gatewayValves = new[]
            {
                context.FK_OK.OpenSignal.PinNumber,
                context.FK_Shl1.OpenSignal.PinNumber,
                context.FK_Shl2.OpenSignal.PinNumber,
            };
            var transportValves = new[]
            {
                context.FK_TM.OpenSignal.PinNumber,
                context.FK_KN1.OpenSignal.PinNumber,
            };
            model.RegisterPressureSimulator(context.TransportLowVacuum.PressureSignal.PinNumber, transportValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.TransportHighVacuum.PressureSignal.PinNumber, transportValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.Gateway1LowVacuum.PressureSignal.PinNumber, gatewayValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.Gateway2LowVacuum.PressureSignal.PinNumber, gatewayValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.AVRLowVacuum.PressureSignal.PinNumber, new[] { context.FK_AVR.OpenSignal.PinNumber },
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.KNGatewaytLowVacuum.PressureSignal.PinNumber, new[] { context.KN2_Zatvor.OpenSignal.PinNumber },
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.KNGatewayHighVacuum.PressureSignal.PinNumber, new[] { context.KN2_Zatvor.OpenSignal.PinNumber },
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.KNTransportLowVacuum.PressureSignal.PinNumber, transportValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.KNTransportHighVacuum.PressureSignal.PinNumber, transportValves,
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);
            model.RegisterPressureSimulator(context.TrupoprovodLowVacuum.PressureSignal.PinNumber, new[] { context.FK_Trb.OpenSignal.PinNumber },
                context.AVR.OilPumpOn.PinNumber, context.AVR.RutsPumpOn.PinNumber, context.AP1.PowerOn.PinNumber);

            model.RegisterDevice<bool>(context.FK_M1.OpenSignal.PinNumber, context.FK_M1.IsOpenSignal.PinNumber, context.FK_M1.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_M2.OpenSignal.PinNumber, context.FK_M2.IsOpenSignal.PinNumber, context.FK_M2.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_M3.OpenSignal.PinNumber, context.FK_M3.IsOpenSignal.PinNumber, context.FK_M3.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_M4.OpenSignal.PinNumber, context.FK_M4.IsOpenSignal.PinNumber, context.FK_M4.IsCloseSignal.PinNumber, 300);

            //model.RegisterDevice<bool>(context.FK_AVR.OpenSignal.PinNumber, context.FK_AVR.IsOpenSignal.PinNumber, context.FK_AVR.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_OK.OpenSignal.PinNumber, context.FK_OK.IsOpenSignal.PinNumber, context.FK_OK.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_AP.OpenSignal.PinNumber, context.FK_AP.IsOpenSignal.PinNumber, context.FK_AP.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_KN1.OpenSignal.PinNumber, context.FK_KN1.IsOpenSignal.PinNumber, context.FK_KN1.IsCloseSignal.PinNumber, 300);

            model.RegisterDevice<bool>(context.KN2_Zatvor.OpenSignal.PinNumber, context.KN2_Zatvor.IsOpenSignal.PinNumber, context.KN2_Zatvor.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.KN_Zatvor_TM.OpenSignal.PinNumber, context.KN_Zatvor_TM.IsOpenSignal.PinNumber, context.KN_Zatvor_TM.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_TM.OpenSignal.PinNumber, context.FK_TM.IsOpenSignal.PinNumber, context.FK_TM.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_Trb.OpenSignal.PinNumber, context.FK_Trb.IsOpenSignal.PinNumber, context.FK_Trb.IsCloseSignal.PinNumber, 300);

            model.RegisterDevice<bool>(context.FK_Shl1.OpenSignal.PinNumber, context.FK_Shl1.IsOpenSignal.PinNumber, context.FK_Shl1.IsCloseSignal.PinNumber, 300);
            model.RegisterDevice<bool>(context.FK_Shl2.OpenSignal.PinNumber, context.FK_Shl2.IsOpenSignal.PinNumber, context.FK_Shl2.IsCloseSignal.PinNumber, 300);
        }
    }
}
