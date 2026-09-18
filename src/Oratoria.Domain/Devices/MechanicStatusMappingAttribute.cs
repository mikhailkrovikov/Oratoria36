using Oratoria.Domain.Devices.Statuses;

namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MechanicStatusMappingAttribute : Attribute
    {
        public readonly MechanicsPositions Position;
        public MechanicStatusMappingAttribute(MechanicsPositions position)
        {
            Position = position;
        }
    }
}
