using Oratoria.Domain.Devices.Errors;

namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Field)] 
    public class MechanicErrorMappingAttribute : Attribute
    {
        public readonly MechanicsErrors Error;
        public MechanicErrorMappingAttribute(MechanicsErrors error)
        {
            Error = error;
        }
    }
}
