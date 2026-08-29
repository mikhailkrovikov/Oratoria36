namespace Oratoria.Domain.Devices.NitrogenFeeder.NitrogenFeederAttributes
{
    public class NitrogenFeederIsPowerOnSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
