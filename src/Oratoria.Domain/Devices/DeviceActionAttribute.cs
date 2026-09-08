namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DeviceActionAttribute : Attribute
    {
        public readonly string Name;
        public DeviceActionAttribute(string name)
        {
            Name = name;
        }
    }
}
