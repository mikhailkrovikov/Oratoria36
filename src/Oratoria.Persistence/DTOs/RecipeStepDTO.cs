namespace Oratoria.Persistence.DTOs
{
    public class RecipeStepDTO
    {
        public int Number { get; set; }
        public Dictionary<string, double> Parameters { get; set; } = new();
    }
}
