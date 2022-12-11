namespace Hero.Server.Core.Models.Storyline
{
    public class StoryEntry
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }

        public string Title { get; set; }
        public string? IconUrl { get; set; }
        public string? Description { get; set; }

        public bool IsUnlocked { get; set; }
        public bool UpdatedAt { get; set; }
        public int Order { get; set; }
    }
}
