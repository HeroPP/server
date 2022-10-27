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
        public DbSet<AttributeCharacter> AttributeCharacters { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(HeroDbResources.Schema);
            builder.ApplyConfigurationsFromAssembly(typeof(HeroDbContext).Assembly);
            builder.Entity<AttributeCharacter>().HasKey(ac => new { ac.AttributeId, ac.CharacterId });

            builder.Entity<AttributeCharacter>()
                .HasOne<Attribute>(ac => ac.Attribute)
                .WithMany(a => a.AttributeCharcters)
                .HasForeignKey(ac => ac.AttributeId);

            builder.Entity<AttributeCharacter>()
                .HasOne<Character>(ac => ac.Character)
                .WithMany(a => a.AttributeCharcters)
                .HasForeignKey(ac => ac.CharacterId);
        }
    }
}
