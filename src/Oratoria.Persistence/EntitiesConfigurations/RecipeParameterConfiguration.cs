using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RecipeParameterConfiguration : IEntityTypeConfiguration<RecipeParameterEntity>
    {
        public void Configure(EntityTypeBuilder<RecipeParameterEntity> builder)
        {
            builder.HasKey(r => r.ParameterId);
            builder.Property(r => r.Name).IsRequired();

            builder
                .HasOne(r => r.Step)
                .WithMany(r => r.Parameters)
                .HasForeignKey(r => r.StepId);
        }
    }
}
