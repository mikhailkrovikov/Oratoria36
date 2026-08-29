namespace Oratoria.Domain.Devices.Leaker.LeakerAttributes
{
    public class LeakerIsCloseSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
