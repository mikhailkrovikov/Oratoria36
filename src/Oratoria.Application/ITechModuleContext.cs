using Oratoria.Domain.Devices.Flap;
using Oratoria.Domain.Devices.Leaker;
using Oratoria.Domain.Devices.Manipulator;
using Oratoria.Domain.Devices.Shutter;
using Oratoria.Domain.Devices.Valve;

namespace Oratoria.Application
{
    public interface ITechModuleContext
    {
        public Valve FK_KN_DU_63 { get; set; }
        public Shutter Shutter { get; set; }
        public Flap Flap { get; set; }
        public Leaker ArgonLeaker { get; set; }
        public Leaker NitrogenLeaker { get; set; }
        public Manipulator Manipulator{ get; set; }
    }
}
