namespace Oratoria.Domain.Devices
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DeviceSignalAttribute<TDevice> : Attribute
    {
        public readonly TDevice DeviceId;
        public DeviceSignalAttribute(TDevice deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
