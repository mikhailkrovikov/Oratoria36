namespace Oratoria.Domain.Devices.ManualPump
{
    public class ManualPumpIsOnSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
