using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server.DataAccess.Database
{
    public class HeroDbContext : DbContext
    {
        public HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeTree> NodeTrees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<AttributeRace> AttributeRaces { get; set; }
        public DbSet<AttributeSkill> AttributeSkills { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(HeroDbResources.Schema);
            builder.ApplyConfigurationsFromAssembly(typeof(HeroDbContext).Assembly);

            builder.Entity<AttributeRace>().HasKey(ac => new { ac.AttributeId, ac.RaceId });
            builder.Entity<AttributeRace>()
                .HasOne<Attribute>(ac => ac.Attribute)
                .WithMany(a => a.AttributeRaces)
                .HasForeignKey(ac => ac.AttributeId);
            builder.Entity<AttributeRace>()
                .HasOne<Race>(ac => ac.Race)
                .WithMany(c => c.AttributeRaces)
                .HasForeignKey(ac => ac.RaceId);

            builder.Entity<AttributeSkill>().HasKey(ac => new { ac.AttributeId, ac.SkillId });
            builder.Entity<AttributeSkill>()
                .HasOne<Attribute>(ats => ats.Attribute)
                .WithMany(at => at.AttributeSkills)
                .HasForeignKey(ats => ats.AttributeId);
            builder.Entity<AttributeSkill>()
                .HasOne<Skill>(ats => ats.Skill)
                .WithMany(s => s.AttributeSkills)
                .HasForeignKey(ats => ats.SkillId);

        }
    }
}
