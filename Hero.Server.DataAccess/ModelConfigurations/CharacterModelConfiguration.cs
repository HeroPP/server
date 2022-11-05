using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hero.Server.Core.ModelConfigurations
{
    public class CharacterModelConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.ToTable("Characters");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(100);

            builder.Property(c => c.HealthPoints).IsRequired();
            builder.Property(c => c.LightPoints).IsRequired();
            builder.Property(c => c.MovementSpeed).IsRequired();
            builder.Property(c => c.Resistance).IsRequired();
            builder.Property(c => c.OpticalRange).IsRequired();
            builder.Property(c => c.Parry).IsRequired();
            builder.Property(c => c.Dodge).IsRequired();
            builder
                .HasMany(charakter => charakter.Skilltrees)
                .WithOne(tree => tree.Character)
                .HasForeignKey(nodeTree => nodeTree.CharacterId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
