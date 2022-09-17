namespace Hero.Server.Core.Models
{
    public class User
    {
        /// <summary>
        /// Unique ID of the User
        /// </summary>
        public Guid Id { get; set; }

        public List<Character> Characters { get; set; }
    }
}
