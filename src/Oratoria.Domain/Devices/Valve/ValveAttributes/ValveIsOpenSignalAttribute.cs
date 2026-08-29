namespace Oratoria.Domain.Devices.Valve.ValveAttributes
{
    public class ValveIsOpenSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
