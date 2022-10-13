using Hero.Server.Core.Models;
using Hero.Server.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hero.Server.DataAccess.Database
{
    public class HeroDbContext : DbContext
    {
        //private readonly IHttpContextAccessor httpContextAccessor;

        public HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options) { }

        //public HeroDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<HeroDbContext> options) : this (options)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //}

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

            //Guid userId = this.httpContextAccessor.HttpContext!.User.GetUserId();
            //builder.Entity<Group>().HasQueryFilter(g => g.OwnerId ==  userId || g.MemberIds.Contains(userId));
        }
    }
}
