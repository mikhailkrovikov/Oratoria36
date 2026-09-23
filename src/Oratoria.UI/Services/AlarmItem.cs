using Oratoria.Domain.Devices;
using Oratoria.Infrastructure;

namespace Oratoria.UI.Services;

public class AlarmItem
{
    public string Text { get; }

    public DeviceErrorCategory Category { get; }

    public string? Description { get; }

    public bool HasDescription =>
        !string.IsNullOrWhiteSpace(Description);

    public AlarmItem(
        string deviceName,
        Enum error,
        DeviceErrorCategory category,
        string? description)
    {
        Text = $"{deviceName}: {error.GetDescription()}";
        Category = category;
        Description = description;
    }
}