using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RecipeValueConfiguration : IEntityTypeConfiguration<RecipeValueEntity>
    {
        public void Configure(EntityTypeBuilder<RecipeValueEntity> builder)
        {
            builder.HasKey(r => r.ValueId);

            builder
                .HasOne<RecipeParameterEntity>()
                .WithOne(p => p.Value)
                .HasForeignKey<RecipeValueEntity>(r => r.ParameterId)
                .IsRequired();
        }
    }
}
