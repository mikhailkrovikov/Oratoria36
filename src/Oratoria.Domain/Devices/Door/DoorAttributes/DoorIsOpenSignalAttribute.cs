namespace Oratoria.Domain.Devices.Door.DoorAttributes
{
    public class DoorIsOpenSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
