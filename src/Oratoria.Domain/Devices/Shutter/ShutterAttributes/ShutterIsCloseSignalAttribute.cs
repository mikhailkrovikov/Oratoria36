namespace Oratoria.Domain.Devices.Shutter.ShutterAttributes
{
    public class ShutterIsCloseSignalAttribute<TDevice>(TDevice deviceId) : DeviceSignalAttribute<TDevice>(deviceId)
    {
    }
}