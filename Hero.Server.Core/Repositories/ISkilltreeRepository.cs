using Hero.Server.Core.Models;

namespace Hero.Server.Core.Repositories
{
    public interface ISkilltreeRepository
    {
        Task CreateSkilltreeAsync(Skilltree skilltree, CancellationToken cancellationToken = default);
        Task UpdateSkilltreeAsync(Guid id, Skilltree updatedSkilltree, CancellationToken cancellationToken = default);
        Task DeleteSkilltreeAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Skilltree?> GetSkilltreeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Skilltree>> GetAllSkilltreesOfCharacterAsync(Guid charId, CancellationToken cancellationToken = default);
    }
}