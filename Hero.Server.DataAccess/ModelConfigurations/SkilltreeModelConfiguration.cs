﻿using Hero.Server.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hero.Server.DataAccess.ModelConfigurations
{
    internal class SkilltreeModelConfiguration : IEntityTypeConfiguration<Skilltree>
    {
        public void Configure(EntityTypeBuilder<Skilltree> builder)
        {
            builder.ToTable("Skilltrees");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id).HasMaxLength(100);
            
            builder.Property(c => c.IsActiveTree).HasDefaultValue(false);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Points).IsRequired();

            builder
                .HasMany(tree => tree.Nodes)
                .WithOne()
                .HasForeignKey(node => node.SkilltreeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
