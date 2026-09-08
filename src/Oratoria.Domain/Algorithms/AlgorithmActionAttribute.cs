namespace Oratoria.Domain.Algorithms
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AlgorithmActionAttribute : Attribute
    {
        public readonly string Name;
        public AlgorithmActionAttribute(string name)
        {
            Name = name;
        }
    }
}
