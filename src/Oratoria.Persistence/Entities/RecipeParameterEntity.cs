using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RecipeParameterConfiguration))]
    public class RecipeParameterEntity
    {
        public Guid ParameterId { get; set; } = Guid.NewGuid();
        public Guid StepId { get; set; }
        public string Name { get; set; } = null!;
        public RecipeValueEntity Value { get; set; }
        public RecipeStepEntity Step { get; set; }
    }
}
