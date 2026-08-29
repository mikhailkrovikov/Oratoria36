namespace Oratoria.Domain.Devices.NitrogenFeeder.NitrogenFeederAttributes
{
    public class NitrogenFeederIsPowerOffSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
