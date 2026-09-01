using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;
using Oratoria.Persistence.ValueTypes;

namespace Oratoria.Persistence.Entities
{   
    [EntityTypeConfiguration(typeof(UserConfiguration))]
    public class UserEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required Role RoleId { get; set; }
    }
}
