namespace Oratoria.Persistence.DTOs
{
    public class RecipeDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public List<RecipeStepDTO> Steps { get; set; } = new();
    }
}
