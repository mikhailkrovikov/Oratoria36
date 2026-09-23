using Oratoria.Domain.Devices;
using Oratoria.Infrastructure;

namespace Oratoria.UI.Services
{
    public class AlarmItem
    {
        public string Text { get; }

        public DeviceErrorCategory Category { get; }

        public AlarmItem(
            string deviceName,
            Enum error,
            DeviceErrorCategory category)
        {
            Text = $"{deviceName}: {error.GetDescription()}";
            Category = category;
        }
    }
}
