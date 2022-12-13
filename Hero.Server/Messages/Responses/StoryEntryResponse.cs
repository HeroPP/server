namespace Hero.Server.Messages.Responses
{
    public class StoryEntryResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string? IconUrl { get; set; }
        public string? Description { get; set; }

        public bool IsUnlocked { get; set; }
        public int Order { get; set; }
    }

    public class StoryImageResponse : StoryEntryResponse
    {
        public string ImageUrl { get; set; }
    }

    public class StoryBookResponse : StoryEntryResponse
    {
        public List<StoryBookPageResponse> Pages { get; set; }
    }
}
