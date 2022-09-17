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
    public class SkillModelConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");
            builder.HasKey(x => x.Id);

            builder
                .HasOne(skill => skill.Ability)
                .WithMany(ability => ability.Skills)
                .HasForeignKey(skill => skill.AbilityId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
