namespace Oratoria.Domain.Devices.Valve.ValveAttributes
{
    public class ValveIsCloseSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
