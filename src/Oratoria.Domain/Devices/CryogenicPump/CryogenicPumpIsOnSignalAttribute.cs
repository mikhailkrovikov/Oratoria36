namespace Oratoria.Domain.Devices.CryogenicPump
{
    public class CryogenicPumpIsOnSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
