namespace Oratoria.Persistence
{
    public class Recipe
    {
        public Guid? Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<Stage> Stages { get; set; } = new();

        public Recipe Copy()
        {
            return new Recipe
            {
                Id = Id,
                ModuleId = ModuleId,
                Name = Name,
                CreatedAt = CreatedAt,
                Stages = Stages.Select(stage => stage.Copy()).ToList()
            };
        }
    }
}
