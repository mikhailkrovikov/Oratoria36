using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RoleConfiguration))]
    public class RoleEntity
    {
        public Role Role { get; set; }
    }
}
