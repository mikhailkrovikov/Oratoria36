using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RecipeConfiguration))]
    public class RecipeEntity
    {
        public Guid RecipeId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public List<RecipeStepEntity> Steps {  get; set; } = new();
    }
}
