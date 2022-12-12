namespace Hero.Server.Core.Models.Storyline
{
    public class StoryBook : StoryEntry
    {
        public List<StoryBookPage> Pages { get; set; }

        public override void Update(StoryEntry updated)
        {
            if (updated is StoryBook updatedBook)
            {
                //foreach (StoryBookPage page in this.Pages.Where(page => updatedBook.Pages.Select(x => x.Id).Contains(page.Id)))
                //{
                //    StoryBookPage updatedPage = updatedBook.Pages.Single(x => x.Id == page.Id);
                //    page.Update(updatedPage);
                //}

                this.Pages.RemoveAll(page => !updatedBook.Pages.Select(x => x.Id).Contains(page.Id));
                this.Pages.AddRange(updatedBook.Pages.Where(page => !this.Pages.Select(x => x.Id).Contains(page.Id)));
            }

            base.Update(updated);
        }
    }
}
