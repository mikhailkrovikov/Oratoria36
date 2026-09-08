namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DeviceActionParameterAttribute : Attribute
    {
        public readonly string Name;
        public DeviceActionParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
