namespace Oratoria.Persistence.DTOs
{
    public class RecipeDTO
    {
        public Guid? Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<RecipeStepDTO> Steps { get; set; } = new();
    }
}
