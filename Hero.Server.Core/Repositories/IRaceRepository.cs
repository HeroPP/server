using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface IRaceRepository
    {
        Task<Race?> GetRaceByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Race>> GetAllRacesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task CreateRaceAsync(Race race, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateRaceAsync(Guid id, Race updatedRace, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteRaceAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    }
}
