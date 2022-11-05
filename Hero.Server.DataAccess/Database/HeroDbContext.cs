﻿using Hero.Server.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Hero.Server.DataAccess.Database
{
    public class HeroDbContext : DbContext
    {
        internal HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options) { }

        public HeroDbContext(GroupContext currentGroup, DbContextOptions<HeroDbContext> options) : base(options)
        {
            this.CurrentGroup = currentGroup;
        }


        public GroupContext CurrentGroup { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Skilltree> Skilltrees { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(HeroDbResources.Schema);
            builder.ApplyConfigurationsFromAssembly(typeof(HeroDbContext).Assembly);

            builder.Entity<Group>().HasQueryFilter(g => g.Id == this.CurrentGroup.Id);
            builder.Entity<Character>().HasQueryFilter(c => c.GroupId == this.CurrentGroup.Id);
            builder.Entity<Ability>().HasQueryFilter(a => a.GroupId == this.CurrentGroup.Id);
            builder.Entity<Skill>().HasQueryFilter(s => s.GroupId == this.CurrentGroup.Id);
            builder.Entity<Skilltree>().HasQueryFilter(s => s.GroupId == this.CurrentGroup.Id);
        }
    }
}
