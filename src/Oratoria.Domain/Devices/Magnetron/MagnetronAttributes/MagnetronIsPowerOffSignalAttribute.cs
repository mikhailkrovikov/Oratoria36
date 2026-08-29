namespace Oratoria.Domain.Devices.Magnetron.MagnetronAttributes
{
    public class MagnetronIsPowerOffSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
