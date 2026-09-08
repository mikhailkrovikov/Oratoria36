using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RecipeStepConfiguration))]
    public class RecipeStepEntity
    {
        public Guid StepId { get; set; } = Guid.NewGuid();
        public Guid RecipeId { get; set; }
        public int Number { get; set; }
        public List<RecipeParameterEntity> Parameters { get; set; } = new();
        public RecipeEntity Recipe { get; set; }
    }
}
