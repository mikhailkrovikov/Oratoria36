namespace Oratoria.Domain.Devices.Leaker.LeakerAttributes
{
    public class LeakerCloseSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
