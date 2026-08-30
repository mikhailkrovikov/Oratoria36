using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.EntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Login).IsRequired();
            builder.Property(u => u.Password).IsRequired();

            builder
                .HasOne<RoleEntity>()
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
