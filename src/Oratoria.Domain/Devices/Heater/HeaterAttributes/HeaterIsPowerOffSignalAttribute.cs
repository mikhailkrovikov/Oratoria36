namespace Oratoria.Domain.Devices.Heater.HeaterAttributes
{
    public class HeaterIsPowerOffSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
