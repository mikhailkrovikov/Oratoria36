using System.Reflection;
using Oratoria.Domain.Devices.Abstractions;

namespace Oratoria.UI.Services;

internal static class DeviceReflection
{
    public static Type? GetDeviceInterface(Type type) =>
        type.GetInterfaces().FirstOrDefault(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDevice<,>));

    public static bool IsDevice(object? obj) =>
        obj != null && GetDeviceInterface(obj.GetType()) != null;

    public static IEnumerable<object> CollectDevices(params object?[] sources)
    {
        var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
        foreach (var source in sources)
        {
            if (source == null)
                continue;

            if (IsDevice(source))
            {
                if (seen.Add(source))
                    yield return source;
                continue;
            }

            foreach (var prop in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.GetIndexParameters().Length != 0)
                    continue;

                object? value;
                try
                {
                    value = prop.GetValue(source);
                }
                catch
                {
                    continue;
                }

                if (value != null && IsDevice(value) && seen.Add(value))
                    yield return value;
            }
        }
    }
}
