using Hero.Server.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.DataAccess.ModelConfigurations
{
    internal class NodeTreeModelConfiguration : IEntityTypeConfiguration<NodeTree>
    {
        public void Configure(EntityTypeBuilder<NodeTree> builder)
        {
            builder.ToTable("NodeTrees");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id).HasMaxLength(100);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Points).IsRequired();
            builder
.HasMany(nodeTree => nodeTree.AllNodes)
.WithOne(node => node.NodeTree)
.HasForeignKey(node => node.NodeTreeId)
.OnDelete(DeleteBehavior.SetNull);
        }
    }
}
