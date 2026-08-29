namespace Oratoria.Domain.Devices.Shutter.ShutterAttributes
{
    public class ShutterIsOpenSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}