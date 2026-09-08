using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<RecipeEntity>
    {
        public void Configure(EntityTypeBuilder<RecipeEntity> builder)
        {
            builder.HasKey(r => r.RecipeId);
        }
    }
}
