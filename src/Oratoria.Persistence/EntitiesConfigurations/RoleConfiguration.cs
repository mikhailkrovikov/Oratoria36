using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(r => r.Role);

            builder.Property(r => r.Role)
                .HasConversion<int>()
                .ValueGeneratedNever();

            builder.HasData
                (new RoleEntity { Role = Role.None },
                new RoleEntity { Role = Role.Operator },
                new RoleEntity { Role = Role.Technologist },
                new RoleEntity { Role = Role.Servicer },
                new RoleEntity { Role = Role.Admin });
        }
    }
}
