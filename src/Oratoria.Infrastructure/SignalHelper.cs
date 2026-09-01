using System.Reflection;

namespace Oratoria.Infrastructure
{
    public static class SignalHelper<T>
        where T : class
    {
        private static string _field = "DeviceId";

        public static T? GetSignal(Enum id, IEnumerable<T> signalCollection, Type signalAttribute)
        {
            var collection = (object)signalCollection;
            var property = collection.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.GetCustomAttributes(false).Any(a =>
                    a.GetType().IsGenericType &&
                    a.GetType().GetGenericTypeDefinition() == signalAttribute &&
                    Equals(
                        a.GetType().GetField(_field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)!.GetValue(a), id)));
            return property?.GetValue(collection) as T;
        }
    }
}