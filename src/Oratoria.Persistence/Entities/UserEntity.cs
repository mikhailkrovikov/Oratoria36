using Microsoft.EntityFrameworkCore;
using Oratoria.Persistence.EntitiesConfigurations;

namespace Oratoria.Persistence.Entities
{   
    [EntityTypeConfiguration(typeof(UserConfiguration))]
    public class UserEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public RoleEntity Role { get; set; }
    }
}
