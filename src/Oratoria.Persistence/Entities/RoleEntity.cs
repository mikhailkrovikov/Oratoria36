using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.Entities
{
    [EntityTypeConfiguration(typeof(RoleConfiguration))]
    public class RoleEntity
    {
        public int Id { get; set; }
        public Role RoleE { get; set; }
        public List<UserEntity> Users { get; set; }
    }
}
