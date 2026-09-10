using Microsoft.Extensions.Logging;
using Oratoria.Application.VacuumModule.DeviceCollection;
using Oratoria.Application.VacuumModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.AVRPump;
using Oratoria.Domain.Devices.CryogenicPump;
using Oratoria.Domain.Devices.NitrogenFeeder;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Settings;
using Oratoria.Infrastructure;

namespace Oratoria.Application.VacuumModule
{
    public class VacuumContext : ModuleContext
    {
        public Valve FK_M1 { get; }

        public Valve FK_M2 { get; }

        public Valve FK_M3 { get; }

        public Valve FK_M4 { get; }

        public Valve FK_AVR { get; }

        public Valve FK_OK { get; }

        public Valve FK_AP { get; }

        public Valve FK_KN1 { get; }

        public Valve KN2_Zatvor { get; }

        public Valve FK_TM { get; }

        public Valve FK_Shl1 { get; }

        public Valve FK_Shl2 { get; }

        public Valve KN_Zatvor_TM { get; }

        public Valve FK_Trb { get; }

        public AVRPump AVR { get; }

        public CryogenicPump KN1_TM { get; }

        public CryogenicPump KN2_Shl { get; }

        public NitrogenFeeder AP1 { get; }

        public PressureSensor Module1LowPressure { get; }

        public PressureSensor Module2LowPressure { get; }

        public PressureSensor Module3LowPressure { get; }

        public PressureSensor Module4LowPressure { get; }

        public PressureSensor TransportLowVacuum { get; }

        public PressureSensor TransportHighVacuum { get; }

        public PressureSensor Gateway1LowVacuum { get; }

        public PressureSensor Gateway2LowVacuum { get; }

        public PressureSensor KNTransportLowVacuum { get; }

        public PressureSensor KNTransportHighVacuum { get; }

        public PressureSensor KNGatewaytLowVacuum { get; }

        public PressureSensor KNGatewayHighVacuum { get; }

        public PressureSensor TrupoprovodLowVacuum { get; }

        public PressureSensor AVRLowVacuum { get; }

        public VacuumContext(VacuumSignals signals, ILoggerFactory loggerFactory, ISettingsContext settings) : base(signals, loggerFactory, settings)
        {
            FK_M1 = Factory.CreateDevice<Valve>(Valves.FK_M1);
            FK_M2 = Factory.CreateDevice<Valve>(Valves.FK_M2);
            FK_M3 = Factory.CreateDevice<Valve>(Valves.FK_M3);
            FK_M4 = Factory.CreateDevice<Valve>(Valves.FK_M4);
            FK_AVR = Factory.CreateDevice<Valve>(Valves.FK_AVR);
            FK_OK = Factory.CreateDevice<Valve>(Valves.FK_OK);
            FK_AP = Factory.CreateDevice<Valve>(Valves.FK_AP);
            FK_KN1 = Factory.CreateDevice<Valve>(Valves.FK_KN1);
            KN2_Zatvor = Factory.CreateDevice<Valve>(Valves.KN2_Zatvor);
            FK_Shl1 = Factory.CreateDevice<Valve>(Valves.FK_Shl1);
            FK_Shl2 = Factory.CreateDevice<Valve>(Valves.FK_Shl2);
            KN_Zatvor_TM = Factory.CreateDevice<Valve>(Valves.KN_Zatvor_TM);
            FK_TM = Factory.CreateDevice<Valve>(Valves.FK_TM);
            FK_Trb = Factory.CreateDevice<Valve>(Valves.FK_Trb);

            AVR = Factory.CreateDevice<AVRPump>(Pumps.AVR);
            KN1_TM = Factory.CreateDevice<CryogenicPump>(Pumps.KN1_TM);
            KN2_Shl = Factory.CreateDevice<CryogenicPump>(Pumps.KN2_Shl);
            AP1 = Factory.CreateDevice<NitrogenFeeder>(NitrogenFeeders.NitrogenFeeder1);

            Module1LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module1LowPressure);
            Module2LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module2LowPressure);
            Module3LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module3LowPressure);
            Module4LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module4LowPressure);
            TransportLowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.TransportLowVacuum);
            TransportHighVacuum = Factory.CreateDevice<HighVacuumSensor>(PressureSensors.TransportHighVacuum);
            Gateway1LowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Gateway1LowVacuum);
            Gateway2LowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Gateway2LowVacuum);
            KNTransportLowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.KNTransportLowVacuum);
            KNTransportHighVacuum = Factory.CreateDevice<HighVacuumSensor>(PressureSensors.KNTransportHighVacuum);
            KNGatewaytLowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.KNGatewaytLowVacuum);
            KNGatewayHighVacuum = Factory.CreateDevice<HighVacuumSensor>(PressureSensors.KNGatewayHighVacuum);
            TrupoprovodLowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.TrupoprovodLowVacuum);
            AVRLowVacuum = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.AVRLowVacuum);
        }

        public override string Name => ModuleId.TransportModule.GetDescription();
    }
}
