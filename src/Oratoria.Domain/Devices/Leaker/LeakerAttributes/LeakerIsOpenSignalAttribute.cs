namespace Oratoria.Domain.Devices.Leaker.LeakerAttributes
{
    public class LeakerIsOpenSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
