using Hero.Server.Core.Models.Storyline;

namespace Hero.Server.Core.Repositories
{
    public interface IStoryBookPageRepository
    {
        Task CreateAsync(Guid bookId, StoryBookPage page, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, StoryBookPage page, CancellationToken cancellationToken = default);
        Task UpdatePositionAsync(Guid id, int newPosition, CancellationToken cancellationToken = default);
    }
}
