using DigitalTwin;
using Oratoria.Application.Module2;

namespace Oratoria.Application
{
    public class DTInitializer
    {
        public DTInitializer(Module2Context context, TwinContext twinContext)
        {
            InitModule(context, twinContext);
        }

        public static void InitModule(TechnologyModuleContext context, TwinContext twinContext)
        {
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

            twinContext.TModel.RegisterDevice<bool>(context.FK_KN_DU_63.Open.PinNumber, context.FK_KN_DU_63.IsOpen.PinNumber, context.FK_KN_DU_63.IsClose.PinNumber, 300);
            twinContext.TModel.RegisterDevice<bool>(context.Shutter.Open.PinNumber, context.Shutter.IsOpen.PinNumber, context.Shutter.IsClose.PinNumber, 300);
            twinContext.TModel.RegisterDevice<bool>(context.Flap.Close.PinNumber, context.Flap.IsClose.PinNumber, context.Flap.IsOpen.PinNumber, 300);

            twinContext.TModel.RegisterDevice<bool>(context.NitrogenLeaker.Open.PinNumber, context.NitrogenLeaker.IsOpen.PinNumber, 300);
            twinContext.TModel.RegisterDevice<bool>(context.ArgonLeaker.Open.PinNumber, context.ArgonLeaker.IsOpen.PinNumber, 300);

            twinContext.TModel.RegisterDevice<double>(context.RRG.RRGSetpointSignal.PinNumber, context.RRG.RRGRealValueSignal.PinNumber, 1000);

            twinContext.TModel.RegisterDevice<bool>(context.Heater.PowerOn.PinNumber, context.Heater.IsPowerOn.PinNumber, 600);
            twinContext.TModel.RegisterDevice<double>(context.Heater.HeaterPowerSetPoint.PinNumber, context.Heater.HeaterVoltage.PinNumber, 600);

            twinContext.TModel.RegisterDevice<bool>(context.Magnetron1.RotationOn.PinNumber, context.Magnetron1.IsRotating.PinNumber, 600);
            twinContext.TModel.RegisterDevice<bool>(context.Magnetron1.PowerOn.PinNumber, context.Magnetron1.IsPowerOn.PinNumber, 600);
            twinContext.TModel.RegisterDevice<double>(context.Magnetron1.MagnetronPowerSetPoint.PinNumber, context.Magnetron1.MagnetronVoltage.PinNumber, 600);
            twinContext.TModel.RegisterDevice<bool>(context.Magnetron2.PowerOn.PinNumber, context.Magnetron2.IsPowerOn.PinNumber, 600);
            twinContext.TModel.RegisterDevice<double>(context.Magnetron2.MagnetronPowerSetPoint.PinNumber, context.Magnetron2.MagnetronVoltage.PinNumber, 600);
            twinContext.TModel.RegisterDevice<bool>(context.Magnetron3.PowerOn.PinNumber, context.Magnetron3.IsPowerOn.PinNumber, 600);
            twinContext.TModel.RegisterDevice<double>(context.Magnetron3.MagnetronPowerSetPoint.PinNumber, context.Magnetron3.MagnetronVoltage.PinNumber, 600);

            twinContext.TModel.RegisterMechanicDevice<bool>(manOuts, manIns, 500);
            twinContext.TModel.RegisterMechanicDevice<bool>(tabOuts, tabIns, 500);
            twinContext.TModel.RegisterMechanicDevice<bool>(tvOuts, tvIns, 500);
        }
    }
}
