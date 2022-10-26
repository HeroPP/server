namespace Hero.Server.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? GroupId { get; set; }

        public List<Character> Characters { get; set; }
        public Group OwnedGroup { get; set; }
        public Group? Group { get; set; }
    }
}
