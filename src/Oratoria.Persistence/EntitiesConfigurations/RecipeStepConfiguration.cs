using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStepEntity>
    {
        public void Configure(EntityTypeBuilder<RecipeStepEntity> builder)
        {
            builder.HasKey(r => r.StepId);

            builder
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(r => r.RecipeId);
        }
    }
}
