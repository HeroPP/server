namespace Hero.Server.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? GroupId { get; set; }

        public List<Character> Characters { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Skill> Skills { get; set; }
        public Group OwnedGroup { get; set; }
    }
}
