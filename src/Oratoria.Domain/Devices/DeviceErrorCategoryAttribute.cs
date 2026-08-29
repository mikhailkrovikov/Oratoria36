namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DeviceErrorCategoryAttribute : Attribute
    {
        public DeviceErrorCategory Category { get; }
        public DeviceErrorCategoryAttribute(DeviceErrorCategory category)
        {
            Category = category;
        }
    }
}
