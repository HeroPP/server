using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Hero.Server.DataAccess.Database
{
    public class HeroDbContext : DbContext
    {
        public HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeTree> NodeTrees { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(HeroDbResources.Schema);
            builder.ApplyConfigurationsFromAssembly(typeof(HeroDbContext).Assembly);
        }
    }
}
