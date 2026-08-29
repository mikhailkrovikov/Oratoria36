namespace Oratoria.Domain.Devices.Heater.HeaterAttributes
{
    public class HeaterIsPowerOnSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}
