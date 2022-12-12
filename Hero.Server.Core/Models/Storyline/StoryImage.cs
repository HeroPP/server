namespace Hero.Server.Core.Models.Storyline
{
    public class StoryImage : StoryEntry
    {
        public string ImageUrl { get; set; }

        public override void Update(StoryEntry updated)
        {
            this.IconUrl = updated.IconUrl;
            base.Update(updated);
        }
    }
}
