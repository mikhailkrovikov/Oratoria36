namespace Oratoria.Domain.Devices.Magnetron.MagnetronAttributes
{
    public class MagnetronIsPowerOnSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
