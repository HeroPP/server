using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Hero.Server.DataAccess.Database
{
    public class HeroDbContext : DbContext
    {
        public HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(HeroDbResources.Schema);
            builder.ApplyConfigurationsFromAssembly(typeof(HeroDbContext).Assembly);
        }
    }
}
