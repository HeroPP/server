namespace Hero.Server.Core.Models.Storyline
{
    public class StoryBookPage
    {
        public int PageNumber { get; set; }
        public Guid BookId { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public bool IsWritten { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}