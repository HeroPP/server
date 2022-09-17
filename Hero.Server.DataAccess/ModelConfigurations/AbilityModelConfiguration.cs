using Hero.Server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.DataAccess.ModelConfigurations
{
    public class AbilityModelConfiguration : IEntityTypeConfiguration<Ability>
    {
        public void Configure(EntityTypeBuilder<Ability> builder)
        {
            builder.ToTable("Abilities");
            builder.HasKey(x => x.Name);
            builder.Property(c => c.Name).HasMaxLength(100);

            builder.Property(c => c.IsPassive).IsRequired();
        }
    }
}
