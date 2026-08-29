using System.Reflection;

namespace Oratoria.Infrastructure
{
    public static class SignalHelper<T>
        where T : class
    {
        public static T? GetSignal(Enum id, IEnumerable<T> signalCollection, Type signalAttribute)
        {
            var collection = (object)signalCollection;
            var property = collection.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.GetCustomAttributes(inherit: false).Any(a =>
                    a.GetType().IsGenericType &&
                    a.GetType().GetGenericTypeDefinition() == signalAttribute &&
                    Equals(
                        a.GetType().GetField("DeviceId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)!.GetValue(a),
                        id)));
            return property?.GetValue(collection) as T;
        }
    }
}