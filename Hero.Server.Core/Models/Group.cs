namespace Hero.Server.Core.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InviteCode { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public List<User> Users { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Skill> Skills { get; set; }

    }
}
