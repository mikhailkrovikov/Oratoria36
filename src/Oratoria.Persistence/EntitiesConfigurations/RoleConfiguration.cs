using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasData
                (new RoleEntity { Id = 0, RoleE = Role.None },
                new RoleEntity { Id = 1, RoleE = Role.Operator },
                new RoleEntity { Id = 2, RoleE = Role.Technologist },
                new RoleEntity { Id = 3, RoleE = Role.Servicer },
                new RoleEntity { Id = 4, RoleE = Role.Admin });
        }
    }
}
