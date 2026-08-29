namespace Oratoria.Domain.Devices.Door.DoorAttributes
{
    public class DoorIsCloseSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
