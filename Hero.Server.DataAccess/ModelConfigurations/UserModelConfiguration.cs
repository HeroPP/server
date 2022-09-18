using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hero.Server.DataAccess.ModelConfigurations
{
    public class UserModelConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder
                .HasMany(user => user.Characters)
                .WithOne()
                .HasForeignKey(character => character.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(user => user.Abilities)
                .WithOne()
                .HasForeignKey(ability => ability.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(user => user.Skills)
                .WithOne()
                .HasForeignKey(skill => skill.UserId);

        }
    }
}
