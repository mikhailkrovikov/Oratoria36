using Microsoft.Extensions.Logging;
using Oratoria.Application.VacuumModule.DeviceCollection;
using Oratoria.Application.VacuumModule.Signals;
using Oratoria.Domain.Abstractions;
using Oratoria.Domain.Devices.ManualPump;
using Oratoria.Domain.Devices.NitrogenFeeder;
using Oratoria.Domain.Devices.PressureSensor;
using Oratoria.Domain.Devices.Valve;
using Oratoria.Domain.Signals.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oratoria.Application.VacuumModule
{
    public class VacuumContext : ModuleContext
    {
        public Valve FK_M1 { get; set; }

        public Valve FK_M2 { get; set; }

        public Valve FK_M3 { get; set; }

        public Valve FK_M4 { get; set; }

        public Valve FK_AVR { get; set; }

        public Valve FK_OK { get; set; }

        public Valve FK_AP { get; set; }

        public Valve FK_KN1 { get; set; }

        public Valve KN2_Zatvor { get; set; }

        public Valve FK_TM { get; set; }

        public Valve FK_Shl1 { get; set; }

        public Valve FK_Shl2 { get; set; }

        public Valve KN_Zatvor_TM { get; set; }

        public Valve FK_Trb { get; set; }

        public AVRPump AVR { get; set; }

        public CryogenicPump KN1_TM { get; set; }

        public CryogenicPump KN2_Shl { get; set; }

        public NitrogenFeeder AP1 { get; set; }

        public PressureSensor Module1LowPressure { get; set; }

        public PressureSensor Module2LowPressure { get; set; }

        public PressureSensor Module3LowPressure { get; set; }

        public PressureSensor Module4LowPressure { get; set; }

        public VacuumContext(VacuumSignals signals, ILoggerFactory loggerFactory) : base(signals, loggerFactory)
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
            FK_Trb = Factory.CreateDevice<Valve>(Valves.FK_Trb);
            AVR = Factory.CreateDevice<AVRPump>(ManualPumps.AVR);
            KN1_TM = Factory.CreateDevice<CryogenicPump>(ManualPumps.KN1_TM);
            KN2_Shl = Factory.CreateDevice<CryogenicPump>(ManualPumps.KN2_Shl);
            AP1 = Factory.CreateDevice<NitrogenFeeder>(NitrogenFeeders.NitrogenFeeder);
            Module1LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module1LowPressure);
            Module2LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module2LowPressure);
            Module3LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module3LowPressure);
            Module4LowPressure = Factory.CreateDevice<LowVacuumSensor>(PressureSensors.Module4LowPressure);
        }
    }
}
