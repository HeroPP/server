using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hero.Server.DataAccess.ModelConfigurations
{
    public class NodeModelConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            builder.ToTable("Nodes");
            builder.HasKey(x => x.Id);

            builder
                .HasOne(node => node.Skill)
                .WithMany(skill => skill.Nodes)
                .HasForeignKey(node => node.SkillId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(n => n.Importance).IsRequired();
            builder.Property(n => n.Cost).IsRequired();
            builder.Property(n => n.XPos).IsRequired();
            builder.Property(n => n.YPos).IsRequired();
            builder.Property(n => n.Color).IsRequired();
            builder.Property(n => n.IsUnlocked).IsRequired();
            builder.Property(n => n.IsEasyReachable).IsRequired();
            builder.Property(n => n.Precessors).IsRequired();
            builder.Property(n => n.Successors).IsRequired();
        }
    }
}
